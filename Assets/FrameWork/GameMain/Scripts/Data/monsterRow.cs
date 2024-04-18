using System.IO;

namespace BFramework
{
    public class monsterRow:IRow
    {
        public string name;
public int attack;
public int hp;
public int def;
public string png;

          
        public monsterRow(BinaryReader binaryReader,int columnNum)
        {
            columnNum = columnNum - 1;
            if(columnNum>=1){
name = binaryReader.ReadString();}

if(columnNum>=2){
attack = binaryReader.ReadInt32();}

if(columnNum>=3){
hp = binaryReader.ReadInt32();}

if(columnNum>=4){
def = binaryReader.ReadInt32();}

if(columnNum>=5){
png = binaryReader.ReadString();}


  
        }
        public void init(){
            if (!Config.Type.TryGetValue("monster",out var type))
            {
                Config.Type.Add("monster",typeof(monsterRow));
            }
        }
    }
}
