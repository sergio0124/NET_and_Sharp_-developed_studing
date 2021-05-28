using LawFirmBusinessLogic.BusinessLogic;
using LawFirmBusinessLogic.HelperModels;
using LawFirmBusinessLogic.Interfaces;
using LawFirmDatabaseImplement.Implements;
using System.Threading;
using System;
using System.Configuration;
using System.Windows.Forms;
using Unity;
using Unity.Lifetime;
using System.Collections.Generic;
using LawFirmBusinessLogic.Attributes;

namespace LawFirmView
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var container = BuildUnityContainer();
            MailLogic.MailConfig(new MailConfig
            {
                SmtpClientHost = ConfigurationManager.AppSettings["SmtpClientHost"],
                SmtpClientPort =
Convert.ToInt32(ConfigurationManager.AppSettings["SmtpClientPort"]),
                MailLogin = ConfigurationManager.AppSettings["MailLogin"],
                MailPassword = ConfigurationManager.AppSettings["MailPassword"],
            });
            // создаем таймер
            var timer = new System.Threading.Timer(new TimerCallback(MailCheck), new
           MailCheckInfo
            {
                PopHost = ConfigurationManager.AppSettings["PopHost"],
                PopPort = Convert.ToInt32(ConfigurationManager.AppSettings["PopPort"]),
                Storage = container.Resolve<IMessageInfoStorage>()
            }, 0, 100000);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(container.Resolve<FormMain>());
        }
        private static IUnityContainer BuildUnityContainer()
        {
            var currentContainer = new UnityContainer();
            currentContainer.RegisterType<IBlankStorage, BlankStorage>(new
           HierarchicalLifetimeManager());
            currentContainer.RegisterType<IOrderStorage, OrderStorage>(new
           HierarchicalLifetimeManager());
            currentContainer.RegisterType<IDocumentStorage, DocumentStorage>(new
           HierarchicalLifetimeManager());
            currentContainer.RegisterType<IClientStorage, ClientStorage>(new
           HierarchicalLifetimeManager());
            currentContainer.RegisterType<IImplementerStorage, ImplementerStorage>(new
           HierarchicalLifetimeManager());
            currentContainer.RegisterType<IMessageInfoStorage, MessageInfoStorage>(new
           HierarchicalLifetimeManager());
            currentContainer.RegisterType<BackUpAbstractLogic, BackUpLogic>(new
           HierarchicalLifetimeManager());
            currentContainer.RegisterType<ImplementerLogic>(new
          HierarchicalLifetimeManager());
            currentContainer.RegisterType<WorkModeling>(new
          HierarchicalLifetimeManager());
            currentContainer.RegisterType<BlankLogic>(new
           HierarchicalLifetimeManager());
            currentContainer.RegisterType<OrderLogic>(new
           HierarchicalLifetimeManager());
            currentContainer.RegisterType<DocumentLogic>(new
           HierarchicalLifetimeManager());
            currentContainer.RegisterType<IStorageStorage, StorageStorage>(new
           HierarchicalLifetimeManager());
            currentContainer.RegisterType<StorageLogic>(new
           HierarchicalLifetimeManager());
            currentContainer.RegisterType<ReportLogic>(new
                HierarchicalLifetimeManager());
            currentContainer.RegisterType<MailLogic>(new
               HierarchicalLifetimeManager());
            return currentContainer;
        }
        private static void MailCheck(object obj)
        {
            MailLogic.MailCheck((MailCheckInfo)obj);
        }

        public static void ConfigGrid<T>(List<T> data, DataGridView grid)
        {
            var type = typeof(T);
            var config = new List<string>();
            grid.Columns.Clear();
            foreach (var prop in type.GetProperties())
            {
                // получаем список атрибутов
                var attributes =
                prop.GetCustomAttributes(typeof(ColumnAttribute), true);
                if (attributes != null && attributes.Length > 0)
                {
                    foreach (var attr in attributes)
                    {
                        // ищем нужный нам атрибут
                        if (attr is ColumnAttribute columnAttr)
                        {
                            config.Add(prop.Name);
                            var column = new DataGridViewTextBoxColumn
                            {
                                Name = prop.Name,
                                ReadOnly = true,
                                HeaderText = columnAttr.Title,
                                Visible = columnAttr.Visible,
                                Width = columnAttr.Width,
                                DefaultCellStyle = new DataGridViewCellStyle { Format = columnAttr.Format ?? "" }
                            };
                            if (columnAttr.GridViewAutoSize !=
                            GridViewAutoSize.None)
                            {
                                column.AutoSizeMode =
                                (DataGridViewAutoSizeColumnMode)Enum.Parse(typeof(DataGridViewAutoSizeColumnMode),
                                columnAttr.GridViewAutoSize.ToString());
                            }
                            grid.Columns.Add(column);
                        }
                    }
                }
            }
            // добавляем строки
            foreach (var elem in data)
            {
                List<object> objs = new List<object>();
                foreach (var conf in config)
                {
                    var value =
                    elem.GetType().GetProperty(conf).GetValue(elem);
                    objs.Add(value);
                }
                grid.Rows.Add(objs.ToArray());
            }
        }
    }
}
