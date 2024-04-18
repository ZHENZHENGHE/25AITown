using System.IO;

namespace BFramework
{
    public class skillRow:IRow
    {
        public string name;
public int num;
public int attack;

          
        public skillRow(BinaryReader binaryReader,int columnNum)
        {
            columnNum = columnNum - 1;
            if(columnNum>=1){
name = binaryReader.ReadString();}

if(columnNum>=2){
num = binaryReader.ReadInt32();}

if(columnNum>=3){
attack = binaryReader.ReadInt32();}


  
        }
        public void init(){
            if (!Config.Type.TryGetValue("skill",out var type))
            {
                Config.Type.Add("skill",typeof(skillRow));
            }
        }
    }
}
