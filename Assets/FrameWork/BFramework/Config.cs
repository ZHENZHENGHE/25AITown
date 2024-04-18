using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

public interface IRow
{
    void init();
}
public class Config
{
    private static int rawRowCount = 0;
    private const string DataTablePath = "Assets/GameMain/Table";
    private const string CSharpCodePath = "Assets/GameMain/Scripts/Data";
    private const string CSharpCodeTemplateFileName = "Assets/GameMain/Configs/DataTableCodeTemplate.txt";
    public  static Dictionary<string,Dictionary<int,IRow>> Data = new Dictionary<string,Dictionary<int,IRow>>();
    public  static Dictionary<string,Type> Type = new Dictionary<string,Type>();
    private static readonly char[] DataSplitSeparators = new char[] { '\t' };
    private static readonly char[] DataTrimSeparators = new char[] { '\"' };
    public static readonly string[] DataTableNames = new string[]
    {
        "role",
        "skill",
        "monster",
        "card"
    };
    public static IRow GetRow(string table,int id)
    {
        var rows = GetTable(table);
        if (table!=null)
        {
            if(rows.TryGetValue(id,out var row))
            {
                return row;
            }
        }
        return null;
    }
    public static Dictionary<int,IRow> GetTable(string table)
    {
        if (Data.TryGetValue(table,out var rows))
        {
            return rows;
        }
        return null;
    }
    public static void  Clear()
    {
        Type.Clear();
        Data.Clear();
    }
    public static void Init()
    {
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
        List<Type> implementingClasses = new List<Type>();
        foreach (var assembly in assemblies)
        {
            Type[] types = assembly.GetTypes()
                .Where(t => t.GetInterfaces().Contains(typeof(IRow)))
                .ToArray();

            implementingClasses.AddRange(types);
        }
        foreach (var implementingClass in implementingClasses)
        {
            if (!Type.TryGetValue(implementingClass.Name,out var type))
            {
                Type.Add(implementingClass.Name,implementingClass);
            }
        }
    }
    public static  void GenerateCodeFile(string dataTableName,string[][] table)
    {
        var m_CodeTemplate = File.ReadAllText(CSharpCodeTemplateFileName, Encoding.UTF8);
        if (string.IsNullOrEmpty(m_CodeTemplate))
        {
            Debug.Log("m_CodeTemplate is null");
            return;
        }
        string csharpCodeFileName = Path.Combine(CSharpCodePath,  dataTableName+"Row"+ ".cs").Replace('\\', '/');
        if (!FileisExists(csharpCodeFileName))
        {
            Debug.Log(dataTableName+"FileDel");
            File.Delete(csharpCodeFileName);
        }
        if (string.IsNullOrEmpty(csharpCodeFileName))
        {
            Debug.Log("csharpCodeFileName  is null");
            return;
        }
        try
        {
            StringBuilder stringBuilder = new StringBuilder(m_CodeTemplate);
            DataTableCodeGenerator(stringBuilder, dataTableName,table);
            using (FileStream fileStream = new FileStream(csharpCodeFileName, FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter stream = new StreamWriter(fileStream, Encoding.UTF8))
                {
                    stream.Write(stringBuilder.ToString());
                }
            }
            Debug.Log("GenerateCodeFile "+dataTableName+" is success");
        }
        catch (Exception exception)
        {
            Debug.LogError("GenerateCodeFile "+dataTableName+" failure, exception is "+exception);
        }
    }
  
    private static void DataTableCodeGenerator(StringBuilder codeContent, object userData,string[][] table)
    {
        string dataTableName = (string)userData;
        codeContent.Replace("__CLASS_NAME__", dataTableName+"Row");
        codeContent.Replace("__DATA_TABLE_PROPERTIES__", GenerateDataTableProperties(table));
        codeContent.Replace("__FUNC__", GenerateDataTableFunc(table));
        codeContent.Replace("__TABLE_NAME__", dataTableName);
    }
    private static string GenerateDataTableFunc(string[][] table)
    {
        StringBuilder stringBuilder = new StringBuilder();
        for (int i = 1; i < table[0].Length; i++)
        {
            string type = "";
            if (table[1][i] == "string")
            {
                type = "ReadString()";
            }
            if (table[1][i] == "int")
            {
                type = "ReadInt32()";
            }
            if (string.IsNullOrEmpty(type))
            {
                Debug.LogError("type is null");
            }
            stringBuilder
                .AppendFormat("if(columnNum>={0})", i)
                .AppendLine("{")
                .AppendFormat("{0} = binaryReader.{1};",table[0][i],type)
                .AppendLine("}").AppendLine();;
        }
        return stringBuilder.ToString();
    }
    private static string GenerateDataTableProperties(string[][] table)
    {
        StringBuilder stringBuilder = new StringBuilder();

        for (int i = 1; i < table[0].Length; i++)
        {
            stringBuilder.AppendFormat("public {0} {1};", table[1][i], table[0][i]).AppendLine();
        }
        return stringBuilder.ToString();
    }
    public static string GetRegularPath(string dataTableName)
    {
        var s = Path.Combine(DataTablePath, dataTableName + ".txt");
        if (s == null)
        {
            return null;
        }
        
        return s.Replace('\\', '/');
    }

    public static bool FileisExists(string dataTableFileName)
    {
        
        if (!File.Exists(dataTableFileName))
        {
            return true;
        }

        return false;
    }

    public static void LoadConfigs(string[] dataTableNames)
    {
        foreach (var dataTableName in dataTableNames)
        {
            LoadConfig(dataTableName);
        }
    }

    public static void LoadConfig(string dataTableName)
    {
        if (Data.ContainsKey(dataTableName))
        {
            return;
        }
        string binaryDataFileName = Path.Combine(DataTablePath, dataTableName + ".bytes").Replace('\\', '/');
        using (FileStream fileStream = new FileStream(binaryDataFileName, FileMode.Open, FileAccess.Read))
        {
            using (BinaryReader binaryReader = new BinaryReader(fileStream, Encoding.UTF8))
            {
                var  table = new Dictionary<int,IRow>();
                var rowNum = binaryReader.ReadInt32();
                for (int i = 0; i < rowNum-2; i++)
                {
                    var columnNum = binaryReader.ReadInt32(); var id = binaryReader.ReadInt32();
                    if (Type.TryGetValue(dataTableName+"Row",out var type))
                    {
                        var obj = (IRow)Activator.CreateInstance(type,binaryReader,columnNum);
                        table.Add(id,obj);
                    }
                }
                if (Data.TryGetValue(dataTableName,out var t))
                {
                    Data[dataTableName] = t;
                }
                else
                {
                    Data.Add(dataTableName,table);
                }
            }
        }
    }
    public static string[][] TxtToArrary(string dataTableFileName)
    {
        string[] lines = File.ReadAllLines(dataTableFileName, Encoding.GetEncoding("UTF-8"));
        rawRowCount = lines.Length;
        List<string[]> rawValues = new List<string[]>();
        for (int i = 0; i < lines.Length; i++)
        {
            string[] rawValue = lines[i].Split(DataSplitSeparators);
            for (int j = 0; j < rawValue.Length; j++)
            {
                rawValue[j] = rawValue[j].Trim(DataTrimSeparators);
            }
            rawValues.Add(rawValue);
        }
        return rawValues.ToArray();
    }

    public static void GenerateDataFile(string dataTableName,string[][] values)
    {
      
        string binaryDataFileName = Path.Combine(DataTablePath, dataTableName + ".bytes").Replace('\\', '/');
        if (string.IsNullOrEmpty(binaryDataFileName))
        {
            return;
        }
        if (File.Exists(binaryDataFileName))
        {
            File.Delete(binaryDataFileName);
            return;
        }
        try
        {
            using (FileStream fileStream = new FileStream(binaryDataFileName, FileMode.Create, FileAccess.Write))
            {
                using (BinaryWriter binaryWriter = new BinaryWriter(fileStream, Encoding.UTF8))
                {
                    binaryWriter.Write(rawRowCount);
                    for (int i = 2; i < rawRowCount; i++)
                    {
                        var num = 0;
                        foreach (var V in values[i])
                        {
                            if (!string.IsNullOrEmpty(V))
                            {
                                num = num + 1;
                            }
                        }
                        binaryWriter.Write(num);
                        for (int j = 0; j < values[i].Length; j++)
                        {
                            if (!string.IsNullOrEmpty(values[i][j]))
                            {
                                if (values[1][j]=="string")//扩展不同类型变量
                                {
                                    binaryWriter.Write(values[i][j]);  
                                }
                                if (values[1][j]=="int")
                                {
                                    if (int.TryParse(values[i][j], out int bytes))
                                    {
                                        binaryWriter.Write(bytes);
                                    }
                                }
                            }
                        }
                        
                    }
                }
            }
            Debug.Log("Parse data table "+dataTableName+" success.");
        }
        catch (Exception exception)
        {
            Debug.LogError("Parse data table "+dataTableName+" failure, exception is "+exception);
            File.Delete(binaryDataFileName);
        }
       
    }

}