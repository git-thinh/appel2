using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace appel
{ 
    public class api_settingApp : api_base, IAPI
    {
        const string file_name = "setting.bin";
        static readonly object _lock = new object();
        static oSetting _setting = new oSetting() { };

        public api_settingApp()
        {
            if (File.Exists(file_name))
            {
                lock (_lock)
                {
                    using (var file = File.OpenRead(file_name))
                    {
                        _setting = Serializer.Deserialize<oSetting>(file);
                        string path_pkg = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "package");
                        if (Directory.Exists(path_pkg))
                        {
                            var ns = Directory.GetFiles(path_pkg, "*.pkg")
                                .Select(x => new oNode()
                                {
                                    anylatic = false,
                                    name = Path.GetFileName(x).Substring(0, Path.GetFileName(x).Length - 4),
                                    content = string.Empty,
                                    path = x,
                                    title = Path.GetFileName(x).Substring(0, Path.GetFileName(x).Length - 4),
                                    type = oNodeType.PACKAGE,
                                })
                                .ToArray();
                            api_nodeStore.Adds(ns);
                            _setting.list_package = ns.Select(x => x.id).ToList();
                        }

                        string path_book = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "book");
                        if (Directory.Exists(path_book))
                        {
                            //Directory.GetFiles(path, "*.txt, *.html")
                            var ps = "*.txt|*.pdf|*.html|*.htm|*.doc|*.docx|*.xls|*.xlsx|*.ppt|*.pptx".Split('|')
                             .SelectMany(filter => System.IO.Directory.GetFiles(path_book, filter))
                             .Select(x => node_Parse(x)).Where(x => x != null).ToArray();
                            api_nodeStore.Adds(ps);
                            _setting.list_book = ps.Select(x => x.id).ToList();
                        }

                        _setting.list_folder = new List<string>() { @"E:\data_el2\articles-IT\w2ui" };

                        f_api_Inited(new msg() { API = _API.SETTING_APP, KEY = _API.SETTING_APP_KEY_INT });
                    }
                }
            }
        }

        private oNode node_Parse(string path_file)
        {
            if (!File.Exists(path_file)) return null;

            oNode node = new oNode();
            node.path = path_file;
            node.name = Path.GetFileName(path_file);

            switch (Path.GetExtension(path_file).ToLower())
            {
                case ".txt":
                    node.type = oNodeType.TEXT;
                    node.content = File.ReadAllText(path_file);
                    node.title = node.content.Split(new char[] { '\r', '\n' })[0];
                    break;
                case ".pdf":
                    node.type = oNodeType.PDF;
                    node.title = node.name.Substring(0, node.name.Length - 4).Trim();
                    break;
                case ".html":
                    node.type = oNodeType.HTML;
                    node.title = node.name.Substring(0, node.name.Length - 5).Trim();
                    break;
                case ".htm":
                    node.type = oNodeType.HTM;
                    node.title = node.name.Substring(0, node.name.Length - 4).Trim();
                    break;
                case ".doc":
                    node.type = oNodeType.DOC;
                    node.title = node.name.Substring(0, node.name.Length - 4).Trim();
                    break;
                case ".docx":
                    node.type = oNodeType.DOCX;
                    node.title = node.name.Substring(0, node.name.Length - 5).Trim();
                    break;
                case ".xls":
                    node.type = oNodeType.XLS;
                    node.title = node.name.Substring(0, node.name.Length - 4).Trim();
                    break;
                case ".xlsx":
                    node.type = oNodeType.XLSX;
                    node.title = node.name.Substring(0, node.name.Length - 5).Trim();
                    break;
                case ".ppt":
                    node.type = oNodeType.PPT;
                    node.title = node.name.Substring(0, node.name.Length - 4).Trim();
                    break;
                case ".pptx":
                    node.type = oNodeType.PPTX;
                    node.title = node.name.Substring(0, node.name.Length - 5).Trim();
                    break;
                default:
                    node = null;
                    break;
            }
            return node;
        }

        public static long[] get_package()
        {
            lock (_lock)
                return _setting.list_package.ToArray();
        }

        public static long[] get_book()
        {
            lock (_lock)
                return _setting.list_book.ToArray();
        }

        public static bool get_checkExistFolder(string fol)
        {
            lock (_lock)
                return _setting.list_folder.IndexOf(fol) != -1;
        }

        public static string[] get_listFolder()
        {
            lock (_lock)
                return _setting.list_folder.ToArray();
        }

        public static oNode get_nodeOpening()
        {
            lock (_lock)
                return api_nodeStore.Get(_setting.node_opening);
        }

        public msg Execute(msg m)
        {
            if (m == null || m.Input == null) return m;
            bool hasUpdate = false;

            switch (m.KEY)
            {
                case _API.SETTING_APP_KEY_UPDATE_FOLDER:
                    #region
                    string fol = (string)m.Input;
                    if (!string.IsNullOrEmpty(fol))
                    {
                        fol = fol.ToLower().Trim();
                        lock (_lock)
                        {
                            if (_setting.list_folder.IndexOf(fol) == -1)
                            {
                                _setting.list_folder.Add(fol);
                                hasUpdate = true;
                                app.postMessageToService(new msg() { API = _API.FOLDER_ANYLCTIC, Input = fol });
                            }
                        }
                    }
                    #endregion
                    break;
                case _API.SETTING_APP_KEY_UPDATE_NODE_OPENING:
                    oNode node = (oNode)m.Input;
                    lock (_lock)
                        _setting.node_opening = node.id;
                    hasUpdate = true;
                    break;
                case _API.SETTING_APP_KEY_UPDATE_SIZE:
                    oAppSize app_size = (oAppSize)m.Input;
                    lock (_lock)
                    {
                        _setting.app_size = app_size;
                        hasUpdate = true;
                    }
                    break;
            }

            //if (hasUpdate)
            //{
            //    using (var file = File.Create(file_name))
            //    {
            //        Serializer.Serialize<oSetting>(file, _setting);
            //    }
            //}
            m.Output.Ok = hasUpdate;
            m.Output.Data = hasUpdate;
            return m;
        }

        public void Close()
        {
            using (var file = File.Create(file_name))
            {
                Serializer.Serialize<oSetting>(file, _setting);
            }
        }
    }

}
