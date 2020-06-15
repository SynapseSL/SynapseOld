using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using Harmony;
using LiteNetLib;
using LiteNetLib.Utils;

namespace Synapse.Events.Patches
{
    [HarmonyPatch(typeof(CustomLiteNetLib4MirrorTransport), nameof(CustomLiteNetLib4MirrorTransport.ProcessConnectionRequest))]
    public class PreAuthenticationPatch
    {
        public static void Postfix(CustomLiteNetLib4MirrorTransport __instance, ConnectionRequest request)
        {
            var allow = true;

            if (!request.Data.EndOfData)
            {
                Log.Warn("Server is not finished handling Authentication");
                return;
            }

            var userId = CustomLiteNetLib4MirrorTransport.UserIds[request.RemoteEndPoint].UserId;

            Events.InvokePreAuthentication(userId, request, ref allow);

            if (allow)
            {
                request.Accept();
                return;
            }
            var data = new NetDataWriter();
            data.Put((byte) 10);
            request.RejectForce(data);
        }
        
        //TODO: Explain this code to other contrib.
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var codes = new List<CodeInstruction>(instructions);

            foreach (var code in codes.Select((x,i) => new {Value =x, Index = i }))
            {
                if (code.Value.opcode != OpCodes.Callvirt) continue;
                if (codes[code.Index + 2].opcode != OpCodes.Ldstr) continue;
                var strOperand = codes[code.Index + 2].operand as string;
                if (strOperand == "Player {0} preauthenticated from endpoint {1}.")
                {
                    code.Value.opcode = OpCodes.Nop;
                }
            }

            return codes.AsEnumerable();
        }
    }
}