﻿using EPlastBoard.BLL.Interfaces.Cards;
using EPlastBoard.DAL.Entities;
using EPlastBoard.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPlastBoard.BLL.Services.Cards
{
    public class CardService: ICardService
    {
        private readonly IRepositoryWrapper _repoWrapper;

        public CardService(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        public async Task<Card> CreateCard(Card card)
        {
            await  _repoWrapper.Card.CreateAsync(card);
            await  _repoWrapper.SaveAsync();
            return card;

        }

        public async Task<int> DeleteCardAsync(int id)
        {
            var card = (_repoWrapper.Card.GetFirstOrDefaultAsync(x => x.Id == id)).Result;
            _repoWrapper.Card.Delete(card);
            await _repoWrapper.SaveAsync();
            return id;
        }

        public async Task<int> EditCardAsync(Card card)
        {

            var oldCard = await _repoWrapper.Card.GetFirstAsync(x => x.Id == card.Id);
            oldCard.Title = card.Title;
            oldCard.Description = card.Description;

            _repoWrapper.Card.Update(oldCard);
            await _repoWrapper.SaveAsync();

            return card.Id;
        }

        public Task<IEnumerable<Card>> GetAllCardsAsync()
        {
            return  _repoWrapper.Card.GetAllAsync();
        }

        public Task<Card> GetCardByIdAsync(int id)
        {
            return _repoWrapper.Card.GetFirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Card>> GetAllCardByColumnAsync(int columnId)
        {
            return await _repoWrapper.Card.GetAllAsync(c => c.Column.Id == columnId);
        }

        public async Task<IEnumerable<Card>> GetCardByColumnAsync(int id)
        {
            return await _repoWrapper.Card.GetAllAsync(x => x.ColumnId == id);
        }
        public async Task<IEnumerable<Card>> GetCardsByBoardAsync(int id)
        {
            var cards = await _repoWrapper.Card.GetAllAsync(predicate: x => x.Column.BoardId == id, 
                include:
                 source => source.Include(c => c.Column));
             return cards.OrderBy(x => x.Index).ToList();
        }

        public async Task UpdateCardAsync(IEnumerable<Card> cards)
        {
            _repoWrapper.Card.Update(cards);
            await _repoWrapper.SaveAsync();
        }
    }
}
