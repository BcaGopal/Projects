using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Models;
using System.ComponentModel.DataAnnotations;

namespace Model.ViewModels
{
    public class CopyFromExistingDesignViewModel
    {
        [Required,Range(1,int.MaxValue,ErrorMessage="The Design field is required")]
        public int ProductGroupId{ get; set; }
        [Required(ErrorMessage="Design Name field is required")]
        public string ProductGroupName { get; set; }
    }

    public class CopyFromExistingSaleInvoiceViewModel
    {
        [Required, Range(1, int.MaxValue, ErrorMessage = "The Sale Invoice field is required")]
        public int SaleInvoiceHeaderId { get; set; }
        [Required(ErrorMessage = "Sale Invoice field is required"),MaxLength(20)]
        public string SaleInvoiceDocNo { get; set; }
    }

    public class CopyFromExistingDesignConsumptionViewModel
    {
        [Required, Range(1, int.MaxValue, ErrorMessage = "Design field is required")]
        public int ProductId { get; set; }
        [Required(ErrorMessage = "Design field is required")]
        public int ProductGroupId { get; set; }
    }

    public class CopyFromExistingProductConsumptionViewModel
    {
        [Required, Range(1, int.MaxValue, ErrorMessage = "Product field is required")]
        public int FromProductId { get; set; }
        [Required(ErrorMessage = "Product field is required")]
        public int ToProductId { get; set; }
    }

    public class CopyFromExistingDesignColourConsumptionViewModel
    {
        [Required, Range(1, int.MaxValue, ErrorMessage = "Design field is required")]
        public int ProductId { get; set; }
        [Required(ErrorMessage = "Design field is required")]
        public int ProductGroupId { get; set; }

        [Required(ErrorMessage = "Design field is required")]
        public int ColourId { get; set; }
    }
}
