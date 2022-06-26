using System.Collections.Generic;
using UnityEngine;

namespace Character.Service
{
    [CreateAssetMenu(fileName = "Offered services", menuName = "Services/Offered Services", order = 0)]
    public class OfferServices : ScriptableObject
    {
        [SerializeField] List<BaseService> offeredServices = new();

        public List<BaseService> OfferedServices => offeredServices;
    }
}