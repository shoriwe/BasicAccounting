using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicAccounting
{
    public class RegistrySearchCondition
    {
        public bool Default_search { get; set; }
        public string Category { get; set; }
        public string Currency { get; set; }
        public double Amount_from { get; set; }
        public double Amount_to { get; set; }
        public string Type { get; set; }
        public DateTime From_date { get; set; }
        public DateTime To_date { get; set; }
        public string In_description { get; set; }
        private bool Check_date(DateTime date)
        {
            DateTime reference = new DateTime();
            if (this.From_date != reference && this.To_date != reference)
            {
                if (this.From_date <= date && date <= this.To_date)
                {
                    return true;
                } else
                {
                    return false;
                }
            } else if (this.From_date != reference)
            {
                if (this.From_date <= date)
                {
                    return true;
                } else
                {
                    return false;
                }
            } else if (this.To_date != reference)
            {
                if (date <= this.To_date)
                {
                    return true;
                } else
                {
                    return false;
                }
            }
            return true;
        }
        private bool Check_type(string type_)
        {
            if (this.Type != "")
            {
                if (this.Type == type_)
                {
                    return true;
                } else
                {
                    return false;
                }
            }
            return true;
        }
        private bool Check_category(string category)
        {
            if (this.Category != "")
            {
                if (this.Category == category)
                {
                    return true;
                } else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }
        private bool Check_amount(double amount)
        {
            if (this.Amount_from != -1 && this.Amount_to != -1)
            {
                if (this.Amount_from <= amount && amount <= this.Amount_to)
                {
                    return true;
                } else
                {
                    return false;
                }
            } else if (this.Amount_from != -1)
            {
                if (this.Amount_from <= amount)
                {
                    return true;
                } else
                {
                    return false;
                }
            } else if (this.Amount_to != -1)
            {
                if (amount <= this.Amount_to)
                {
                    return true;
                } else
                {
                    return false;
                }
            } else
            {
                return true;
            }
        }
        private bool Check_description(string description)
        {
            if (this.In_description != "")
            {
                string lower_description = description.ToLower();
                string[] reference = this.In_description.ToLower().Split(Convert.ToChar(" "));
                foreach (string word in reference)
                {
                    if (lower_description.Contains(word) != true)
                    {
                        return false;
                    }
                }
                return true;
            } else
            {
                return true;
            }
        }
        private bool Check_currency(string currency)
        {
            if (this.Currency != "")
            {
                if (this.Currency == currency)
                {
                    return true;
                } else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }
        public bool Check_condition(DateTime date, string category, string type_, double amount, string description, string currency)
        {
            if (this.Check_date(date) &&
                this.Check_category(category) &&
                this.Check_type(type_) &&
                this.Check_amount(amount) &&
                this.Check_description(description) &&
                this.Check_currency(currency))
            {
                return true;
            }
            return false;
        }

    }
}
