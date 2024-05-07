using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using Cinemachine;
using StarterAssets;
using UnityEngine.InputSystem;
using System;

public class PlayerCtrl : MonoBehaviour
{
    public static Action<EquipStationCtrl.PlayerProtectionSuitType> SetEquipmentData;

    //Player state of movement
    public enum PlayerState
    {
        IS_IDLE,
        IS_WALKING,
        IS_RUNNING,
        IS_PICKING
    }

    public static PlayerCtrl LocalInstance;

    public Animator playerAnim;

    [SerializeField]
    private float health = 100;
    [SerializeField]
    private float bioHazardLevel = 0;
    [SerializeField]
    private float radiationLevel = 0;

    [SerializeField]
    private PlayerInventorySystem playerInventorySystem;

    [SerializeField]
    private Transform cameraTransform;

    [SerializeField]
    private CinemachineVirtualCamera cinemachineVitualCam;

    [SerializeField]
    private FirstPersonController fpController;

    [SerializeField]
    private PlayerInput platerInput;

    [SerializeField]
    private CharacterController characterController;

    [SerializeField]
    private PlayerEquipmentCtrl playerEquipmentCtrl;

    public GameObject playerInventoryGo;

    [Serializable]
    public struct PlayerCamPoints
    {
        public Transform sleep;
        public Transform wake;
    };

    [SerializeField]
    private PlayerCamPoints playerCamPoints;

    public Transform camRoot;

    [Serializable]
    public struct PlayerSuitMats
    {
        public Material radiationSuit;
        public Material biohazardSuit;
    }

    public PlayerSuitMats playerSuitMats;

    public Renderer playerRenderer;

    public AudioSource playerAudioSource;
    public AudioSource worldAudioSource;
    public AudioSource footStepsAudioSource;

    private bool _isPlayerUiActive = false;

    private int layerMask = 1 << 8;

    public PlayerEquipmentCtrl PlayerEquipment
    {
        get { return playerEquipmentCtrl; }
    }

    public PlayerInventorySystem PlayerInventory
    {
        get { return playerInventorySystem; }
    }

    private PlayerState _state = PlayerState.IS_IDLE;

    public PlayerState CurrentState
    {
        get { return _state; }
    }

    private float _healthToReduceForEachLevelOfRadiation = 0.03f;
    private float _healthToReduceForEachLevelOfBioHazardEffect = 0.06f;

    private float _radiationLevelToIncrease = 1f;
    private float _bioHazardLevelToIncrease = 1f;

    private bool isHealthDecreasing = false;

    private bool _isGameOver = false;

    public float BioHazardLevel
    {
        get { return bioHazardLevel; }
        set { bioHazardLevel = value; }
    }

    public float RadiationLevel
    {
        get { return radiationLevel; }
        set { radiationLevel = value; }
    }

    public float Health
    {
        get { return health; }
    }

    #region NON MULTIPLAYER
    private void Awake()
    {
        LocalInstance = this;
    }

    #endregion

    private void Start()
    {
        layerMask = ~layerMask;
        PlayWorldAudio(Utilities.Instance.gameAudioClips.outWorldWind, shouldLoop: true, m_volume: 1f);
    }

    private void Update()
    {
        if (!isHealthDecreasing && (radiationLevel > 0 || bioHazardLevel > 0))
        {
            ReduceHealthByRadiationOrBioHazardLevelIncrease();
            isHealthDecreasing = true;
        }
        if (isHealthDecreasing && (radiationLevel <= 0 && bioHazardLevel <= 0))
        {
            StopCoroutine("ReduceHealthSlowly");
            isHealthDecreasing = false;
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            _isPlayerUiActive = !_isPlayerUiActive;
            playerInventoryGo.SetActive(_isPlayerUiActive);
        }

        if (!_isGameOver && health <= 0)
        {
            GameManager.Instance.SetGameOver();
            _isGameOver = true;
        }
    }

    private void LateUpdate()
    {
        //When Player Click on the garbage or the Ui, then the actions were called to interact using Ray
        RaycastHit hit;
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, 5f, layerMask))
            {
                if (hit.collider.tag == "Garbage")
                {
                    GarbageCtrl garbageCtrl = hit.collider.GetComponent<GarbageCtrl>();
                    if (garbageCtrl != null && playerEquipmentCtrl.GarbageThatCanBeAddedToInventory.Contains(garbageCtrl.GarbageType)) {
                        playerInventorySystem.AddItemToInventory(garbageCtrl.GarbageType, 1);
                        SetPlayerAnimations(PlayerCtrl.PlayerState.IS_PICKING, 1f);
                        //garbageCtrl.HideObjectServerRpc();
                        garbageCtrl.SetGarbageState(m_garbageState: GarbageCtrl.GarbageState.COLLECTED);
                    }
                }
                if (hit.collider.tag == "Button")
                {
                    Button buttonCom = hit.collider.GetComponent<Button>();
                    buttonCom.onClick.Invoke();
                }
            }
        }
    }

    private void SetPlayerInitialEquipmentData(EquipStationCtrl.PlayerProtectionSuitType suitType)
    {
        playerEquipmentCtrl.SetPlayerEquipment(suitType);
        SetEquipmentData?.Invoke(suitType);
    }

    private void ReduceHealthByRadiationOrBioHazardLevelIncrease()
    {
        StartCoroutine("ReduceHealthSlowly");
    }

    //Reducing health slowly when the hazard level increase
    private IEnumerator ReduceHealthSlowly()
    {
        while (health > 0)
        {
            yield return new WaitForSeconds(1f);
            health -= (radiationLevel * _healthToReduceForEachLevelOfRadiation) + (bioHazardLevel * _healthToReduceForEachLevelOfBioHazardEffect);
            GameManager.Instance.playerStatsUiCtrl.SetHealthInUi(health);
        }

        if (health <= 0)
        {
            health = 0;
            GameManager.Instance.playerStatsUiCtrl.SetHealthInUi(health);
            StopCoroutine("ReduceHealthSlowly");
            isHealthDecreasing = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        InfectPlayerCtrl infectPlayerCtrl = other.GetComponent<InfectPlayerCtrl>();
        if (infectPlayerCtrl != null)
        {
            if ( infectPlayerCtrl.InfectType == GarbageManager.GarbageType.RADIOACTIVE)
            {
                if(playerEquipmentCtrl.PlayerSuit != EquipStationCtrl.PlayerProtectionSuitType.RADIATION)
                {
                    _radiationLevelToIncrease = 1f;
                }
                else
                {
                    _radiationLevelToIncrease = 0.1f;
                }
                StartCoroutine("IncreaseRadiationValueSlowly");
            }
            else if (infectPlayerCtrl.InfectType == GarbageManager.GarbageType.BIO_HAZARD)
            {
                if (playerEquipmentCtrl.PlayerSuit != EquipStationCtrl.PlayerProtectionSuitType.BIO_HAZARD)
                {
                    _bioHazardLevelToIncrease = 1f;
                }
                else
                {
                    _bioHazardLevelToIncrease = 0.1f;
                }
                StartCoroutine("IncreaseBioHazardValueSlowly");
            }
        }

        if(other.tag == "Base"){
            PlayWorldAudio(Utilities.Instance.gameAudioClips.inBaseWind, shouldLoop: true, m_volume: 1f);
            Utilities.Instance.AudioToBePlayedAsFootStep(Utilities.PlayerMovePlace.BASE);
        }
        if(other.tag == "Water"){
            Utilities.Instance.AudioToBePlayedAsFootStep(Utilities.PlayerMovePlace.WATER);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        InfectPlayerCtrl infectPlayerCtrl = other.GetComponent<InfectPlayerCtrl>();

        if (infectPlayerCtrl != null)
        {
            StopInfectLevelCoroutines(infectPlayerCtrl.InfectType);
        }
        if(other.tag == "Base"){
            PlayWorldAudio(Utilities.Instance.gameAudioClips.outWorldWind, shouldLoop: true, m_volume: 1f);
            Utilities.Instance.AudioToBePlayedAsFootStep(Utilities.PlayerMovePlace.SAND);
        }
        if(other.tag == "Water"){
            Utilities.Instance.AudioToBePlayedAsFootStep(Utilities.PlayerMovePlace.SAND);
        }
    }

    public IEnumerator IncreaseBioHazardValueSlowly()
    {
        GameManager gameManager = GameManager.Instance;
        while (bioHazardLevel < 100)
        {
            yield return new WaitForSeconds(1f);
            bioHazardLevel += _bioHazardLevelToIncrease;
            gameManager.playerStatsUiCtrl.SetBioHazardInUi(bioHazardLevel);
        }

        if (bioHazardLevel >= 100)
        {
            bioHazardLevel = 100;
            gameManager.playerStatsUiCtrl.SetBioHazardInUi(bioHazardLevel);
            StopCoroutine("IncreaseBioHazardValueSlowly");
        }
    }

    public IEnumerator IncreaseRadiationValueSlowly()
    {
        while (radiationLevel < 100)
        {
            yield return new WaitForSeconds(1f);
            radiationLevel += _radiationLevelToIncrease;
            GameManager.Instance.playerStatsUiCtrl.SetRadiationInUi(radiationLevel);
        }

        if (radiationLevel >= 100)
        {
            radiationLevel = 100;
            GameManager.Instance.playerStatsUiCtrl.SetRadiationInUi(radiationLevel);
            StopCoroutine("IncreaseRadiationValueSlowly");
        }
    }

    public void StopInfectLevelCoroutines(GarbageManager.GarbageType garbageType)
    {
        if (garbageType == GarbageManager.GarbageType.RADIOACTIVE)
        {
            StopCoroutine("IncreaseRadiationValueSlowly");
        }
        else if (garbageType == GarbageManager.GarbageType.BIO_HAZARD)
        {
            StopCoroutine("IncreaseBioHazardValueSlowly");
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    //Getting player data to save
    public DataManager.PlayerGameData GetPlayerGameData()
    {
        DataManager.PlayerGameData playerGameData = new DataManager.PlayerGameData();
        playerGameData.position = transform.position;
        playerGameData.health = health;
        playerGameData.radiationLevel = radiationLevel;
        playerGameData.bioHazardLevel = bioHazardLevel;
        playerGameData.playerEquipmentType = playerEquipmentCtrl.PlayerSuit.ToString();

        List<DataManager.PlayerInventoryDetails> playerInventoryDetails = new List<DataManager.PlayerInventoryDetails>();
        foreach (KeyValuePair<string, InventorySystem.InventoryItemData> item in playerInventorySystem.GetInventoryItemsData())
        {
            DataManager.PlayerInventoryDetails itemDetails = new DataManager.PlayerInventoryDetails()
            {
                garbageType = item.Key,
                count = item.Value.count
            };
            playerInventoryDetails.Add(itemDetails);
        }

        playerGameData.playerInventoryDetails = playerInventoryDetails;


        return playerGameData;
    }

    //Setting player data at the start
    public void SetPlayerGameData()
    {
        DataManager.UserGameData m_userGameData = DataManager.Instance.GetCurrentUserData();
        if (!m_userGameData.Equals(null))
        {

            DataManager.PlayerGameData playerGameData = DataManager.Instance.GetCurrentUserData().playerGameData;
            transform.position = playerGameData.position;
            health = playerGameData.health;
            radiationLevel = playerGameData.radiationLevel;
            bioHazardLevel = playerGameData.bioHazardLevel;

            EquipStationCtrl.PlayerProtectionSuitType playerProtectionSuitType = EquipStationCtrl.PlayerProtectionSuitType.BIO_HAZARD;
            Enum.TryParse(playerGameData.playerEquipmentType, out playerProtectionSuitType);
            SetPlayerInitialEquipmentData(playerProtectionSuitType);
            for (int i = 0; i < playerGameData.playerInventoryDetails.Count; i++)
            {
                DataManager.PlayerInventoryDetails itemDetails = playerGameData.playerInventoryDetails[i];
                playerInventorySystem.AddItemToInventory((GarbageManager.GarbageType)Enum.Parse(typeof(GarbageManager.GarbageType), itemDetails.garbageType), itemDetails.count);

            }
        }
        else
        {
            SetPlayerInitialEquipmentData(EquipStationCtrl.PlayerProtectionSuitType.RADIATION);
        }
    }

    public void WakeUpPlayer()
    {
        camRoot.SetParent(playerCamPoints.sleep, false);
        camRoot.localPosition = Vector3.zero;
        playerAnim.SetTrigger("wake");
    }

    public void ChangeCamToWake()
    {
        camRoot.SetParent(playerCamPoints.wake);
        camRoot.localPosition = Vector3.zero;
    }

    public void SetPlayerAnimations(PlayerState m_state, float m_speed)
    {
        if (_state != m_state)
        {
            playerAnim.SetFloat("direction", m_speed);
            _state = m_state;
            SetAnimation(m_state);
        }
    }

    //Setting animation state of the player
    public void SetAnimation(PlayerState m_state)
    {
        switch (m_state)
        {
            case PlayerState.IS_WALKING:
                {
                    playerAnim.SetTrigger("IsWalking");
                    break;
                }
            case PlayerState.IS_RUNNING:
                {
                    playerAnim.SetTrigger("IsRunning");
                    break;
                }
            case PlayerState.IS_PICKING:
                {
                    playerAnim.SetTrigger("pick");
                    break;
                }
            case PlayerState.IS_IDLE:
            default:
                {                 
                    playerAnim.SetTrigger("idle");
                    break;
                }
        }
    }

    public void ChangePlayerSuit(bool isBioHazard)
    {

        Material[] materials = playerRenderer.materials;
        if (isBioHazard)
        {
            materials[0] = playerSuitMats.biohazardSuit;
        }
        else
        {
            materials[0] = playerSuitMats.radiationSuit;
        }
        playerRenderer.materials = materials;
    }

#region PLAYER AUDIO
    public void PlayFootStepsAudio(AudioClip m_clip, bool shouldLoop, float m_volume)
    {
        footStepsAudioSource.loop = shouldLoop;
        footStepsAudioSource.volume = m_volume;
        if (m_clip.name != footStepsAudioSource.clip.name)
        {
            footStepsAudioSource.clip = m_clip;
        }
        footStepsAudioSource.Play();
    }

    public void PlayPlayerAudio(AudioClip m_clip, bool shouldLoop, float m_volume)
    {
        playerAudioSource.loop = shouldLoop;
        playerAudioSource.volume = m_volume;
        if (m_clip.name != playerAudioSource.clip.name)
        {
            playerAudioSource.clip = m_clip;
        }
        playerAudioSource.Play();
    }

    public void PlayWorldAudio(AudioClip m_clip, bool shouldLoop, float m_volume)
    {
        worldAudioSource.loop = shouldLoop;
        worldAudioSource.volume = m_volume;
        if (m_clip.name != worldAudioSource.clip.name)
        {
            worldAudioSource.clip = m_clip;
        }
        worldAudioSource.Play();
    }
    #endregion
}
