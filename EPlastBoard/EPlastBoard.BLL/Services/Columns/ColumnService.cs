﻿using EPlastBoard.BLL.Interfaces.Columns;
using EPlastBoard.DAL.Entities;
using EPlastBoard.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EPlastBoard.BLL.Services.Columns
{
    public class ColumnService : IColumnService
    {
        private readonly IRepositoryWrapper _repoWrapper;

        public ColumnService(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        public async Task<Column> GetColumnByIdAsync(int id)
        {
            return await _repoWrapper.Columns.GetFirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<int> EditColumnNameAsync(Column column)
        {
            _repoWrapper.Columns.Attach(column);
            _repoWrapper.Columns.Update(column);
            await _repoWrapper.SaveAsync();

            return column.Id;
        }

        public async Task<Column> CreateColumnAsync(Column column)
        {
            var allColumns = await _repoWrapper.Columns.GetAllAsync();
            if (!allColumns.Contains(column))
            {
                throw new ArgumentException("The same board as exist");
            }
            await  _repoWrapper.Columns.CreateAsync(column);
            await  _repoWrapper.SaveAsync();
            return column;
        }

        public async Task<int> DeleteColumnAsync(int id)
        {
            var column = await _repoWrapper.Columns.GetFirstOrDefaultAsync(x => x.Id == id);
            _repoWrapper.Columns.Delete(column);
            await _repoWrapper.SaveAsync();
            return id;
        }

        public async Task<IEnumerable<Column>> GetAllColumnsByBoardAsync(int boardId)
        {
            return await _repoWrapper.Columns.GetAllAsync(c => c.Board.Id == boardId);
        }
    }
}