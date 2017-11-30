namespace VRTK
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class key : VRTK_InteractableObject
    {

        // Use this for initialization
        void Start()
        {

        }

        public override void StartUsing(VRTK_InteractUse usingObject)
        {
            SceneManager.LoadScene("Handmade_Target_Course_2", LoadSceneMode.Single);
        }

    }
}
