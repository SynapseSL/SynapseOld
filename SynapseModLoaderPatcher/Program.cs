using System;
using System.Linq;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using dnlib.DotNet.Writer;
// ReSharper disable UnusedVariable

namespace SynapseModLoaderPatcher
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Missing File Argument");
                return;
            }

            var module = ModuleDefMD.Load(args[0]);

            if (module == null)
            {
                Console.WriteLine("File Not Found");
                return;
            }

            module.IsILOnly = true;
            module.VTableFixups = null;
            module.Assembly.PublicKey = null;
            module.Assembly.HasPublicKey = false;
            
            var opts = new ModuleWriterOptions(module);
            
            Console.WriteLine($"Synapse: Loaded {module.Name}");
            Console.WriteLine("Synapse-Assemble: Resolving Ref..");

            var modCtx = ModuleDef.CreateModuleContext();
            var asmResolver = (AssemblyResolver) modCtx.AssemblyResolver;

            module.Context = modCtx;

            ((AssemblyResolver) module.Context.AssemblyResolver).AddToCache(module);
            Console.WriteLine("Synapse-Injection: Injection of ModLoader");

            var modLoader = ModuleDefMD.Load("ModLoader.dll");

            Console.WriteLine($"Synapse-Inject: Loaded {modLoader.Name}");

            var modClass = modLoader.Types[0];

            foreach (var type in modLoader.Types)
            {
                if (type.Name != "ModLoader") continue;
                modClass = type;
                Console.WriteLine($"Synapse-Inject: Hooked to: {type.Namespace}.{type.Name}");
            }

            var modRefType = modClass;

            modLoader.Types.Remove(modClass);
            modRefType.DeclaringType = null;
            module.Types.Add(modRefType);

            MethodDef call = FindMethod(modRefType, "LoadModSystem");

            if (call == null)
            {
                Console.WriteLine("Failed to get 'LoadModSystem'. Perm Err?");
                return;
            }

            Console.WriteLine("Synapse-Inject: Injected!");
            Console.WriteLine("Synapse: Patching...");

            TypeDef def = FindType(module.Assembly, "ServerConsoleSender");
            
            MethodDef bctor = new MethodDefUser(".ctor", MethodSig.CreateInstance(module.CorLibTypes.Void),
                MethodImplAttributes.IL | MethodImplAttributes.Managed,
                MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName);

            if (FindMethod(def, ".ctor") != null)
            {
                bctor = FindMethod(def, ".ctor");
                Console.WriteLine("Synapse: Re-using constructor.");
            }
            else def.Methods.Add(bctor);

            CilBody body;
            bctor.Body = body = new CilBody();

            body.Instructions.Add(OpCodes.Call.ToInstruction(call));
            body.Instructions.Add(OpCodes.Ret.ToInstruction());

            module.Write("Assembly-CSharp-Synapse.dll");
            Console.WriteLine("Synapse: Patch Complete!");
        }
        
        private static MethodDef FindMethod(TypeDef type, string methodName)
        {
            if (type == null) return null;
            return type.Methods.FirstOrDefault(method => method.Name == methodName);
        }

        private static TypeDef FindType(AssemblyDef asm, string classPath)
        {
            return asm.Modules.SelectMany(module => module.Types).FirstOrDefault(type => type.FullName == classPath);
        }
    }
}