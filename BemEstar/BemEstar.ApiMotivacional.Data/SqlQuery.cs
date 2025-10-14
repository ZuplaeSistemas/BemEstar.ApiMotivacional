using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BemEstar.ApiMotivacional.Data
{
    public class SqlQuery
    {
        public static string SelectAll(string table) =>
          $"SELECT * FROM {table}";

        public static string SelectById(string table) =>
            $"SELECT * FROM {table} WHERE id = @id";

        public static string Delete(string table) =>
            $"DELETE FROM {table} WHERE id = @id";

        public static string Insert(string table, string[] columns)
        {
            string columnList = string.Join(", ", columns);
            string paramList = string.Join(", ", columns.Select(c => "@" + c));
            return $"INSERT INTO {table} ({columnList}) VALUES ({paramList})";
        }

        public static string Update(string table, string[] columns)
        {
            string setList = string.Join(", ", columns.Select(c => $"{c} = @{c}"));
            return $"UPDATE {table} SET {setList} WHERE id = @id";
        }
    }
}
