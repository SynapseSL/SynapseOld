using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using dnlib.DotNet.Writer;
using FieldAttributes = dnlib.DotNet.FieldAttributes;
using MethodAttributes = dnlib.DotNet.MethodAttributes;
using MethodImplAttributes = dnlib.DotNet.MethodImplAttributes;
using TypeAttributes = dnlib.DotNet.TypeAttributes;

// ReSharper disable UnusedVariable

namespace SynapseModLoaderPatcher
{
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class Program
    {
        /// <summary>
        /// This Program is used to compile the Assembly-CSharp.
        ///
        /// For Instruction on how to use this Program please check the readme of this Project.
        /// </summary>
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

            var modLoader = ModuleDefMD.Load("SynapseModLoader.dll");

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

            var call = FindMethod(modRefType, "LoadModSystem");

            if (call == null)
            {
                Console.WriteLine("Failed to get 'LoadModSystem'. Perm Err?");
                return;
            }

            Console.WriteLine("Synapse-Inject: Injected!");
            Console.WriteLine("Synapse: Patching...");

            var def = FindType(module.Assembly, "ServerConsoleSender");
            
            // ReSharper disable once IdentifierTypo
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
            
            var publicModule = ModuleDefMD.Load(args[0]);

            Console.WriteLine("Synapse-Public: Creating Publicized DLL");

            var allTypes = GetAllTypes(publicModule.Assembly.ManifestModule);
            var typeDefs = allTypes.ToList();
            var allMethods = typeDefs.SelectMany(t => t.Methods);
            var allFields = typeDefs.SelectMany(t => t.Fields);

            #region Publiczing

            foreach (var type in typeDefs)
            {
                if (!type?.IsPublic ?? false)
                {
                    type.Attributes = type.IsNested ? TypeAttributes.NestedPublic : TypeAttributes.Public;
                }
            }

            foreach (var method in allMethods)
            {
                if (!method?.IsPublic ?? false)
                {
                    method.Access = MethodAttributes.Public;
                }
            }


            foreach (var field in allFields)
            {
                if (!field?.IsPublic ?? false)
                {
                    field.Access = FieldAttributes.Public;
                }
            }

            #endregion

            publicModule.Write("Assembly-CSharp-Synapse_publicised.dll");

            Console.WriteLine("Synapse-Public: Created Publicised DLL");

            Thread.Sleep(1000000000);
        }
        
        private static MethodDef FindMethod(TypeDef type, string methodName)
        {
            return type?.Methods.FirstOrDefault(method => method.Name == methodName);
        }

        private static TypeDef FindType(AssemblyDef asm, string classPath)
        {
            return asm.Modules.SelectMany(module => module.Types).FirstOrDefault(type => type.FullName == classPath);
        }

        public static IEnumerable<TypeDef> GetAllTypes(ModuleDef moduleDef)
        {
            return _GetNestedTypes(moduleDef.Types);
        }

        private static IEnumerable<TypeDef> _GetNestedTypes(IEnumerable<TypeDef> typeDefs)
        {
            var enumerable = typeDefs as TypeDef[] ?? typeDefs.ToArray();
            if (enumerable?.Count() == 0)
            {
                return new List<TypeDef>();
            }

            var res = enumerable.Concat(_GetNestedTypes(enumerable.SelectMany(t => t.NestedTypes)));
            return res;
        }
    }
}