using System.IO;

namespace BFramework
{
    public class cardRow:IRow
    {
        public string name;
public int value;
public string des;
public int type;
public int energy;
public string png;
public int needgoal;
public int nextlevel;
public int role;
public int isInit;
public int cost;
public int givelevel;
public int id;

          
        public cardRow(BinaryReader binaryReader,int columnNum)
        {
            columnNum = columnNum - 1;
            if(columnNum>=1){
name = binaryReader.ReadString();}

if(columnNum>=2){
value = binaryReader.ReadInt32();}

if(columnNum>=3){
des = binaryReader.ReadString();}

if(columnNum>=4){
type = binaryReader.ReadInt32();}

if(columnNum>=5){
energy = binaryReader.ReadInt32();}

if(columnNum>=6){
png = binaryReader.ReadString();}

if(columnNum>=7){
needgoal = binaryReader.ReadInt32();}

if(columnNum>=8){
nextlevel = binaryReader.ReadInt32();}

if(columnNum>=9){
role = binaryReader.ReadInt32();}

if(columnNum>=10){
isInit = binaryReader.ReadInt32();}

if(columnNum>=11){
cost = binaryReader.ReadInt32();}

if(columnNum>=12){
givelevel = binaryReader.ReadInt32();}

if(columnNum>=13){
id = binaryReader.ReadInt32();}


  
        }
        public void init(){
            if (!Config.Type.TryGetValue("card",out var type))
            {
                Config.Type.Add("card",typeof(cardRow));
            }
        }
    }
}
