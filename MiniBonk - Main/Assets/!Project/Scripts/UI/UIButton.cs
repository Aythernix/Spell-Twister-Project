
using Minibonk.Management;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Minibonk.UI
{
	public class UIButton : UISystem, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, ISubmitHandler 
	{
		#region Variables

		private bool _isHovered = false;
		private bool _isSelected = false;
		

		#endregion
		
		#region Unity Methods
		void Update()
		{	
			// Detects is element is being selected
			if (EventSystem.current.currentSelectedGameObject == gameObject && !_isHovered && !_isSelected)
			{
				OnUISelect(true);
				_isSelected = true;
				GameManager.Instance.EventHandler.PlayAudio(AudioManager.AudioClips.UIHover, null);
			}
			
			// Detects if element has been unselected after being previously selected
			if (EventSystem.current.currentSelectedGameObject != gameObject &&
			    EventSystem.current.currentSelectedGameObject != null && _isSelected && !_isHovered)
			{
				OnUISelect(false);
				_isSelected = false;
			}
		}
		#endregion

		#region Private Method
		
		// Detects Mouse Hovers
		public void OnPointerEnter(PointerEventData eventData)
		{
			_isHovered = true;

			OnUIHover(true, eventData);

			GameManager.Instance.EventHandler.PlayAudio(AudioManager.AudioClips.UIHover, null);
		}
		
		// Detects Mouse Exits
		public void OnPointerExit(PointerEventData eventData)
		{
			_isHovered = false;

			OnUIHover(false, eventData);
		}
		
		// Mouse Clicks
		public void OnPointerClick(PointerEventData eventData)
		{
			OnUIClick("Mouse",eventData);

			GameManager.Instance.EventHandler.PlayAudio(AudioManager.AudioClips.UIConfirm, null);
		}
		
		// Controller Inputs
		public void OnSubmit(BaseEventData eventData)
		{
			OnUIClick("Controller",eventData);
			
			GameManager.Instance.EventHandler.PlayAudio(AudioManager.AudioClips.UIConfirm, null);
		}

		#endregion

		#region Protected Methods

		/// <summary>
		/// Called when the players cursor hovers over a UI element
		/// </summary>
		/// <param name="isHover">TRUE if hovering</param>
		/// <param name="eventData"></param>
		protected virtual void OnUIHover(bool isHover, PointerEventData eventData)
		{

		}
		/// <summary>
		/// Called when the UI element has been selected
		/// </summary>
		/// <param name="isSelect">TRUE if selected</param>
		protected virtual void OnUISelect(bool isSelect)
		{

		}

		/// <summary>
		/// Called if UI element is clicked with the mouse or Controller
		/// </summary>
		/// <param name="inputMethod"></param>
		/// <param name="eventData"></param>
		protected virtual void OnUIClick(string inputMethod, BaseEventData eventData)
		{

		}

		#endregion
	}
}

