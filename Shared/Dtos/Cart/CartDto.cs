﻿namespace Shared.Dtos.Cart
{
    public class CartDto
    {
        public CartHeaderDto CartHeader { get; set; }
        public List<CartDetailDto>? CartDetails { get; set; }
    }
}
