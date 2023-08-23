﻿using Shopping.Dtos;
using Shopping.Dtos.CartsDtos.RequestDtos;
using Shopping.Dtos.CartsDtos.ResponseDtos;

namespace Shopping.Interfaces
{
    public interface ICartServices
    {
        Task<GeneralResponse<CartDto>> AddToCart(int userId, int cartId, int bookId, AddDto dto);
        Task<GeneralResponse<CartDto>> DeleteFromCart(int userId, int bookId);
        Task<GeneralResponse<List<CartDto>>> ListOfBooksInCart(int id);
        Task<GeneralResponse<List<CartDto>>> Buying(int userId, int cartId);
    }
}