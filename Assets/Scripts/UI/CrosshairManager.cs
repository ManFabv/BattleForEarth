using KBCore.Refs;
using Unity.FPS.Game;
using Unity.FPS.Gameplay;
using UnityEngine;
using UnityEngine.UI;

namespace Unity.FPS.UI
{
    public class CrosshairManager : ValidatedMonoBehaviour
    {
        public Image CrosshairImage;
        public Sprite NullCrosshairSprite;

        [HideInInspector, SerializeField, Self] PlayerWeaponsManager m_WeaponsManager;
        
        bool m_WasPointingAtEnemy;
        RectTransform m_CrosshairRectTransform;
        ShotTypeConfig m_CrosshairDataDefault;
        ShotTypeConfig m_CurrentCrosshair;

        void Start()
        {
            OnWeaponChanged(m_WeaponsManager.GetActiveWeapon());

            m_WeaponsManager.OnSwitchedToWeapon += OnWeaponChanged;
        }

        void Update()
        {
            UpdateCrosshairPointingAtEnemy(false);
            m_WasPointingAtEnemy = m_WeaponsManager.IsPointingAtEnemy;
        }

        void UpdateCrosshairPointingAtEnemy(bool force)
        {
            if (m_CrosshairDataDefault == null)
                return;

            if (m_CrosshairDataDefault.CrosshairSprite == null)
                return;
            
            m_CurrentCrosshair = m_CrosshairDataDefault;

            if ((force || !m_WasPointingAtEnemy) && m_WeaponsManager.IsPointingAtEnemy)
            {
                CrosshairImage.sprite = m_CurrentCrosshair.CrosshairSprite;
                m_CrosshairRectTransform.sizeDelta = m_CurrentCrosshair.CrosshairSize * Vector2.one;
            }
            else if ((force || m_WasPointingAtEnemy) && !m_WeaponsManager.IsPointingAtEnemy)
            {
                CrosshairImage.sprite = m_CurrentCrosshair.CrosshairSprite;
                m_CrosshairRectTransform.sizeDelta = m_CurrentCrosshair.CrosshairSize * Vector2.one;
            }

            int crosshairSize = m_WeaponsManager.IsPointingAtEnemy ? m_CurrentCrosshair.CrosshairAtSigthSize : m_CurrentCrosshair.CrosshairSize;
            float crosshairUpdateshrpness = m_CurrentCrosshair.CrosshairUpdateshrpness;

            CrosshairImage.color = Color.Lerp(CrosshairImage.color, m_CurrentCrosshair.CrosshairColor, Time.deltaTime * crosshairUpdateshrpness);
            m_CrosshairRectTransform.sizeDelta = Mathf.Lerp(m_CrosshairRectTransform.sizeDelta.x, crosshairSize, Time.deltaTime * crosshairUpdateshrpness) * Vector2.one;
        }

        void OnWeaponChanged(WeaponController newWeapon)
        {
            if (newWeapon)
            {
                CrosshairImage.enabled = true;
                m_CrosshairDataDefault = newWeapon.CrosshairDataDefault;
                m_CrosshairRectTransform = CrosshairImage.GetComponent<RectTransform>();
                DebugUtility.HandleErrorIfNullGetComponent<RectTransform, CrosshairManager>(m_CrosshairRectTransform, this, CrosshairImage.gameObject);
            }
            else
            {
                if (NullCrosshairSprite)
                {
                    CrosshairImage.sprite = NullCrosshairSprite;
                }
                else
                {
                    CrosshairImage.enabled = false;
                }
            }

            UpdateCrosshairPointingAtEnemy(true);
        }
    }
}