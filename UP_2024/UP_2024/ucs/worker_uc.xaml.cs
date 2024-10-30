using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UP_2024.db;

namespace UP_2024.ucs
{
    

    public partial class worker_uc : UserControl
    {
        private User user;

        public worker_uc(User _user)
        {
            InitializeComponent();
            user = _user;
            worker_name_tb.Text = user.Surname;


            worker_operation_tb.Text = WriteAllUserOperations();
        }

        private string WriteAllUserOperations()
        {

            List<PerformTasks> operations = App.db.PerformTasks
                .Where(task => App.db.User_tasks
                    .Where(ut => ut.user_id == user.User_id)
                    .Select(ut => ut.id_perform_task)
                    .Contains(task.Id))
                .ToList();

            string allOperations = "";
            for (int i = 0; i < operations.Count; i++)
            {
                if (i == 0)
                    allOperations += operations[i].Name;
                else
                    allOperations += ", " + operations[i].Name;
            }

            return allOperations;
        }

    }

}
