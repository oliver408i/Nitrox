using NitroxModel.Packets;
using NitroxServer.Communication.Packets.Processors.Abstract;
using NitroxServer.GameLogic;
using NitroxServer.GameLogic.Entities;

namespace NitroxServer.Communication.Packets.Processors;

public class AttackCyclopsTargetChangedProcessor : TransmitIfCanSeePacketProcessor<AttackCyclopsTargetChanged>
{
    public AttackCyclopsTargetChangedProcessor(PlayerManager playerManager, EntityRegistry entityRegistry) : base(playerManager, entityRegistry) { }

    public override void Process(AttackCyclopsTargetChanged packet, Player sender) => TransmitIfCanSeeEntities(packet, sender, [packet.CreatureId, packet.TargetId]);
}
