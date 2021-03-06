using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

public partial class InnerWeaver
{
	static Dictionary<string, Assembly> assemblies = new Dictionary<string, Assembly>(StringComparer.OrdinalIgnoreCase);

	public Assembly LoadAssembly(string assemblyPath)
	{
		Assembly assembly;
		if (assemblies.TryGetValue(assemblyPath, out assembly))
		{
            Logger.LogInfo(string.Format("\tLoading '{0}' from cache.", assemblyPath));
			return assembly;
        }
        Logger.LogInfo(string.Format("\tLoading '{0}' from disk.", assemblyPath));
		return assemblies[assemblyPath] = LoadFromFile(assemblyPath);
	}

	public static Assembly LoadFromFile(string assemblyPath)
    {
        var pdbPath = Path.ChangeExtension(assemblyPath, "pdb");
        var rawAssembly = File.ReadAllBytes(assemblyPath);
        if (File.Exists(pdbPath))
        {
            return Assembly.Load(rawAssembly, File.ReadAllBytes(pdbPath));
        }
        var mdbPath = Path.ChangeExtension(assemblyPath, "mdb");
        if (File.Exists(mdbPath))
        {
            return Assembly.Load(rawAssembly, File.ReadAllBytes(mdbPath));
        }
        return Assembly.Load(rawAssembly);
    }

}