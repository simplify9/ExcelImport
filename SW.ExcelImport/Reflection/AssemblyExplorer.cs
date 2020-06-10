using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SW.ExcelImport
{
    public class AssemblyExplorer
    {
        bool Include(AssemblyName asmName)
        {
            return !asmName.FullName.StartsWith("system", StringComparison.InvariantCultureIgnoreCase);
        }

        public IEnumerable<Type> TraverseAppDomain()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var asm in assemblies)
            {
                var types = Traverse(asm);
                foreach (var t in types) yield return t;
            }
            //var mainAsm = Assembly.GetEntryAssembly();
            //return Traverse(mainAsm);
        }

        public IEnumerable<Type> Traverse(Assembly asm)
        {
            var deps = new Stack<Assembly>();
            var crawledSet = new HashSet<string>();
            deps.Push(asm);
            while (deps.Count > 0)
            {
                var iAssembly = deps.Peek();
                // --- visit children ---
                var lRefs = iAssembly.GetReferencedAssemblies();
                if (!crawledSet.Contains(iAssembly.GetName().FullName))
                {
                    foreach (var r in lRefs)
                        if (Include(r) && !crawledSet.Contains(r.FullName))
                        {
                            Assembly loaded;
                            try
                            {
                                loaded = Assembly.Load(r);
                            }
                            catch (FileNotFoundException)
                            {
                                loaded = null;
                            }
                            if (loaded != null) deps.Push(loaded);
                        }
                    crawledSet.Add(iAssembly.GetName().FullName);
                }
                else
                {
                    iAssembly = deps.Pop();
                    var publicTypes = iAssembly.GetTypes();
                    foreach (var type in publicTypes) yield return type;
                }
            }
        }
    }
}
