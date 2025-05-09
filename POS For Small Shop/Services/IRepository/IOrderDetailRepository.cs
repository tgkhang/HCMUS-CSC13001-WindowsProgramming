﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POS_For_Small_Shop.Data.Models;

namespace POS_For_Small_Shop.Services.IRepository
{
    public interface IOrderDetailRepository<OrderDetail> : IRepository<OrderDetail>
    {
        public Receipt getReceiptDetailByOrderId(int orderId);
    }
}
