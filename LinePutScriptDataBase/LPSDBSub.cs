using System.IO.MemoryMappedFiles;
using LinePutScript;

namespace LinePutScriptDataBase
{
    public static class LPSDBSub
    {
        public static DataBase GetLPSDB(string DBName, string password = "")
        {
            DBName = DBName.ToUpper();
            MemoryMappedViewAccessor viewAccessor = MemoryMappedFile.OpenExisting("lpsdb" + DBName).CreateViewAccessor();
            viewAccessor.Read(0, out DataBaseLocker locker);
            return locker.GetDataBase(password);
        }
        public static LpsDocument GetLPS(string DBName, string password = "")
            => GetLPSDB(DBName, password).LPS;
    }
}
