using System.IO;

namespace BFramework
{
    public class roleRow:IRow
    {
        public string name;
public int hp;
public int def;
public string png;

          
        public roleRow(BinaryReader binaryReader,int columnNum)
        {
            columnNum = columnNum - 1;
            if(columnNum>=1){
name = binaryReader.ReadString();}

if(columnNum>=2){
hp = binaryReader.ReadInt32();}

if(columnNum>=3){
def = binaryReader.ReadInt32();}

if(columnNum>=4){
png = binaryReader.ReadString();}


  
        }
        public void init(){
            if (!Config.Type.TryGetValue("role",out var type))
            {
                Config.Type.Add("role",typeof(roleRow));
            }
        }
    }
}
