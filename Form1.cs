using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FiltrSearchSort44App.ModelEF;

namespace FiltrSearchSort44App
{
    public partial class Form1 : Form
    {
        Model1 db = new Model1();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            List<string> lstCategory = db.Categories.Select(c => c.CategoryName).ToList();
            lstCategory.Sort();
            lstCategory.Insert(0, "Все категории");
            FiltrCombo.DataSource = lstCategory;
            FiltrCombo.SelectedIndex = 0;
            SortCombo.SelectedIndex = 0;

            suppliersBindingSource.DataSource = db.Suppliers.ToList();
            categoriesBindingSource.DataSource = db.Categories.ToList();
            productsBindingSource.DataSource = db.Products.ToList();
        }

        string filter = "Все категории";
        string search = "";
        string sort = "Без сортировки";
        void PodgotovkaData()
        {
            // фильтрация
            List<Products> lstProducts = db.Products.ToList();
            if(filter != "Все категории")
            {
                lstProducts = lstProducts.Where(p => p.Categories.
                                        CategoryName == filter).ToList();
            }
            // поиск
            if(search != "")
            {
                search = search.ToUpper();
                lstProducts = lstProducts.
                    Where(p => p.ProductName.ToUpper().Contains(search)).ToList();
            }

            if(sort != "Без сортировки")
            {
                if(sort == "Название")
                {
                    if (!SortCheck.Checked)
                    {
                        lstProducts = lstProducts.OrderBy(p => p.ProductName).ToList();
                    }
                    else 
                    {
                        lstProducts = lstProducts.OrderByDescending(p => p.ProductName).ToList();
                    }
                }
                else if(sort == "Цена")
                {
                    if (!SortCheck.Checked)
                    {
                        lstProducts = lstProducts.OrderBy(p => p.UnitPrice).ToList();
                    }
                    else
                    {
                        lstProducts = lstProducts.OrderByDescending(p => p.UnitPrice).ToList();
                    }
                }    

            }

            productsBindingSource.DataSource = lstProducts;
 
        }
        private void FiltrCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            filter = FiltrCombo.Text;
            PodgotovkaData();
        }

        private void SearchTxt_TextChanged(object sender, EventArgs e)
        {
            search = SearchTxt.Text;
            PodgotovkaData();
        }

        private void SortCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            sort = SortCombo.Text;
            PodgotovkaData();
        }

        private void SortCheck_CheckedChanged(object sender, EventArgs e)
        {
            PodgotovkaData();
        }
    }
}
