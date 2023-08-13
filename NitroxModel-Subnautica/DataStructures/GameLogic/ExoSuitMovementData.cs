using System;
using System.Runtime.Serialization;
using BinaryPack.Attributes;
using NitroxModel.DataStructures;
using NitroxModel.DataStructures.GameLogic;
using NitroxModel.DataStructures.Unity;

namespace NitroxModel_Subnautica.DataStructures.GameLogic
{
    [Serializable]
    [DataContract]
    public class ExosuitMovementData : VehicleMovementData
    {
        [DataMember(Order = 1)]
        public NitroxVector3 LeftAimTarget { get; }

        [DataMember(Order = 2)]
        public NitroxVector3 RightAimTarget { get; }

        [IgnoreConstructor]
        protected ExosuitMovementData()
        {
            // Constructor for serialization. Has to be "protected" for json serialization.
        }

        public ExosuitMovementData(NitroxTechType techType, NitroxId id, NitroxVector3 position, NitroxQuaternion rotation, float steeringWheelYaw, float steeringWheelPitch, bool appliedThrottle, NitroxVector3 leftAimTarget, NitroxVector3 rightAimTarget, NitroxVector3 driverPosition = default, NitroxQuaternion driverRotation = default)
            : base(techType, id, position, rotation, steeringWheelYaw, steeringWheelPitch, appliedThrottle, driverPosition, driverRotation)
        {
            LeftAimTarget = leftAimTarget;
            RightAimTarget = rightAimTarget;
        }
        
        public override string ToString()
        {
            return $"[ExosuitMovementData - {base.ToString()}, LeftAimTarget: {LeftAimTarget}, RightAimTarget: {RightAimTarget}]";
        }
    }
}
