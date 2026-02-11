
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Minibonk.UI
{
	public class UIButton : UISystem, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, ISubmitHandler 
	{
		#region Variables

		private bool isHovered = false;
		private bool isSelected = false;

		protected AudioClip _UIHoverSound;
		protected AudioClip _UISelectSound;
		protected AudioClip _UIClickSound;

		#endregion

		void Update()
		{	
			// Detects is element is being selected
			if (EventSystem.current.currentSelectedGameObject == gameObject && !isHovered)
			{
				OnUISelect(true);
				isSelected = true;
				if (_UISelectSound != null)
				{
					//TODO Call UI Play Sound Event
				}
			}
			
			// Detects if element has been unselected after being previously selected
			if (EventSystem.current.currentSelectedGameObject != gameObject &&
			    EventSystem.current.currentSelectedGameObject != null && isSelected && !isHovered)
			{
				OnUISelect(false);
				isSelected = false;
			}
		}

		#region Private Method
		
		// Detects Mouse Hovers
		public void OnPointerEnter(PointerEventData eventData)
		{
			isHovered = true;

			OnUIHover(true, eventData);

			if (_UIHoverSound != null)
			{
				//TODO Call UI Play Sound Event
			}
		}
		
		// Detects Mouse Exits
		public void OnPointerExit(PointerEventData eventData)
		{
			isHovered = false;

			OnUIHover(false, eventData);
		}
		
		// Mouse Clicks
		public void OnPointerClick(PointerEventData eventData)
		{
			OnUIClick("Mouse",eventData);

			if (_UIClickSound != null)
			{
				//TODO Call UI Play Sound Event
			}
		}
		
		// Controller Inputs
		public void OnSubmit(BaseEventData eventData)
		{
			OnUIClick("Controller",eventData);
			
			if (_UIClickSound != null)
			{
				//TODO Call UI Play Sound Event
			}
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

