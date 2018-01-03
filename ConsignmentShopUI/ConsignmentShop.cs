using ConsignmentShopLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConsignmentShopUI
{
    public partial class ConsignmentShop : Form
    {
        //store has list of vendors and list of items
        private Store store = new Store();
        private List<Item> shoppingCartData = new List<Item>();
        BindingSource itemsBinding = new BindingSource();
        BindingSource cartBinding = new BindingSource();
        BindingSource vendorsBinding = new BindingSource();

        private decimal storeProfit = 0;

        public ConsignmentShop()
        {
            InitializeComponent();
            SetUpData();
            itemsBinding.DataSource = store.Items.Where(x => x.Sold == false).ToList(); 
            itemsListBox.DataSource = itemsBinding;

            itemsListBox.DisplayMember = "Display";
            itemsListBox.ValueMember = "Display";

            cartBinding.DataSource = shoppingCartData;
            ShoppingCartListBox.DataSource = cartBinding;

            ShoppingCartListBox.DisplayMember = "Display";
            ShoppingCartListBox.ValueMember = "Display";

            vendorsBinding.DataSource = store.Vendors;
            vendorListBox.DataSource = vendorsBinding;

            vendorListBox.DisplayMember = "Display";
            vendorListBox.ValueMember = "Display";
        }

        private void SetUpData()
        {
            //Vendor vendor1 = new Vendor();
            //vendor1.FirstName = "Preetham";
            //vendor1.LastName = "Munshi";
            //vendor1.Commission = 0.5;

            //vendor1 = new Vendor();
            //vendor1.FirstName = "Geoff";
            //vendor1.LastName = "Wasmuth";
            //vendor1.Commission = 0.2;

            //store.Vendors.Add(vendor1);

            //below is simple code instead of 9 lines of code above.

            store.Vendors.Add(new Vendor { FirstName = "Preetham", LastName = "Munshi" });
            store.Vendors.Add(new Vendor { FirstName = "Geoffrey", LastName = "Wasmuth" });
            store.Vendors.Add(new Vendor { FirstName = "Joe", LastName = "Best" });

            store.Items.Add(new Item
            {
                Title = "BB8",
                Description = "The Force Awakens Droid",
                Price = 149.99M,
                Owner = store.Vendors[0]
            });
            store.Items.Add(new Item
            {
                Title = "R2 D2",
                Description = "A New Hope Droid",
                Price = 149.99M,
                Owner = store.Vendors[0]
            });
            store.Items.Add(new Item
            {
                Title = "BrainLess Man",
                Description = "A painting of brainless man",
                Price = 9.99M,
                Owner = store.Vendors[2]
            });
            store.Items.Add(new Item
            {
                Title = "Ipad",
                Description = "A hand machine",
                Price = 699.99M,
                Owner = store.Vendors[1]
            });

            store.Name = "Impulsive Buying";

        }

     

        private void addToCart_Click(object sender, EventArgs e)
        {
            /*Figure out what is selected from the Items List 
             Copy that Item to shopping cart
             Do we remove the item from Items list - NO */

            Item selectedItem = (Item)itemsListBox.SelectedItem;

            //MessageBox.Show(selectedItem.Title);

            shoppingCartData.Add(selectedItem);

            cartBinding.ResetBindings(false); //true if you want to change schema
    
        }

        
        private void makePurchase_Click(object sender, EventArgs e)
        {
            /* Mark each item in the cart as sold
               Clear the cart*/

            foreach(Item item in shoppingCartData)
            {
                item.Sold = true;
                item.Owner.PaymentDue += (decimal)item.Owner.Commission * item.Price;
                storeProfit += (1 - ((decimal)item.Owner.Commission)) * item.Price;
            }

            shoppingCartData.Clear();

            itemsBinding.DataSource = store.Items.Where(x => x.Sold == false).ToList();

            storeProfitValue.Text = string.Format("${0}", storeProfit);

            cartBinding.ResetBindings(false);
            itemsBinding.ResetBindings(false);

            vendorsBinding.ResetBindings(false);
        }

    


    }
}
