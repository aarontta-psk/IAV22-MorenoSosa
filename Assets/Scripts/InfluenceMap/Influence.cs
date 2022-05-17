using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace es.ucm.fdi.iav.rts
{

    public class Influence : MonoBehaviour
    {

        // Cantidad de presencia que la unidad representa en el mapa
        [SerializeField] private float _influence = 1f;
        
        public float radius_ = 1.0f;

        private void Start()
        {
            //if(gameObject.GetComponent<Unit>() != null){
            //    radius_ = gameObject.GetComponent<Unit>().Radius;
            //}else if(gameObject.GetComponent<Tower>() != null){
            //    radius_ = gameObject.GetComponent<Tower>().Radius;
            //}else if(gameObject.GetComponent<Village>() != null){
            //    radius_ = gameObject.GetComponent<Village>().Radius;
            //}else if(gameObject.GetComponent<Facility>() != null){
            //    radius_ = gameObject.GetComponent<Facility>().Radius;
            //}
        }

        public virtual float LinearDropoff(float distance)
        {
            float d = _influence / radius_ * distance;
            return _influence - d;
        }
        public virtual float LongerDropoff(float distance)
        {
            float d = _influence / radius_ * (float)Math.Sqrt(distance);
            return _influence - d;
        }
        public virtual float ShorterDropoff(float distance)
        {
            float d = _influence / radius_ * (distance * distance);
            return _influence - d;
        }
    }
}