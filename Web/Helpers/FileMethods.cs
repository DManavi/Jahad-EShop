using System;
using System.IO;
using System.Web;

namespace Web
{
    public static class FileMethods
    {
        public static void Save(this HttpPostedFileBase file, string path, HttpServerUtilityBase server, Guid id)
        {
            path = CalculatePath(path, id, server);

            file.SaveAs(path);
        }

        public static void Delete(string path, HttpServerUtilityBase server, Guid id)
        {
            path = CalculatePath(path, id, server);

            File.Delete(path);
        }

        public static string Address(string path, Guid id)
        {
            return CalculatePath(path, id);
        }

        private static string CalculatePath(string path, Guid id, HttpServerUtilityBase server = null)
        {
            if (server != null)
            {
                path = string.Format("~/Content/Uploads/{0}", path);

                path = server.MapPath(path);

                if (!Directory.Exists(path)) { Directory.CreateDirectory(path); }
            }
            else
            {
                path = string.Format("/Content/Uploads/{0}", path);
            }

            path = string.Format("{0}/{1}.jpg", path, id.ToString("N"));

            return path;
        }
    }
}