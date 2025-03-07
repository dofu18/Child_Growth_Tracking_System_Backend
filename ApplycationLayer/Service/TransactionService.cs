using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.ResponseCode;
using ApplicationLayer.DTOs.Payments;
using ApplicationLayer.DTOs.RatingFeedback;
using ApplicationLayer.DTOs.Transaction;
using ApplicationLayer.Shared;
using AutoMapper;
using DomainLayer.Entities;
using InfrastructureLayer.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static DomainLayer.Enum.GeneralEnum;

namespace ApplicationLayer.Service
{
    public interface ITracsactionService
    {
        Task<IActionResult> GetMyTransaction(TransactionQuery query, PaymentStatusEnum? status);
    }

    public class TransactionService : BaseService, ITracsactionService
    {
        private readonly IGenericRepository<Transaction> _repo;

        public TransactionService(IGenericRepository<Transaction> repo, IMapper mapper, IHttpContextAccessor httpCtx) : base(mapper, httpCtx)
        {
            _repo = _repo;
        }

        public async Task<IActionResult> GetMyTransaction(TransactionQuery query, PaymentStatusEnum? status)
        {
            //string searchKeyword = query.SearchKeyword ?? "";
            DateOnly? fromDate = query.FromDate;
            DateOnly? toDate = query.ToDate;
            int page = query.Page < 0 ? 0 : query.Page;
            int pageSize = query.PageSize <= 0 ? 10 : query.PageSize;

            var payload = ExtractPayload();
            if (payload == null)
            {
                return ErrorResp.Unauthorized("Invalid token");
            }

            var filter = PredicateBuilder.True<Transaction>();

            var userId = payload.UserId;

            filter = filter.And(t => t.UserId.Equals(userId));

            if (fromDate.HasValue || toDate.HasValue)
            {
                var from = fromDate ?? DateOnly.MinValue;
                var to = toDate ?? DateOnly.MaxValue;

                filter = filter.And(r => DateOnly.FromDateTime(r.PaymentDate) >= from
                                    && DateOnly.FromDateTime(r.PaymentDate) <= to);
            }

            if (status != null)
            {
                filter = filter.And(u => u.PaymentStatus == status);
            }

            var transactions = await _repo.WhereAsync(
                filter,
                orderBy: q => q.OrderByDescending(u => u.CreatedAt),
                page: page,
                pageSize: pageSize,
                "User", "Package"
            );

            var totalTransactions = await _repo.CountAsync(filter);
            //var transactions = await _repo.WhereAsync
            //  .Skip(page * pageSize)
            //  .Take(pageSize)
            //  .ToList();

            //var transaction = _mapper.Map<IEnumerable<TransactionDto>>(transactions);

            var result = new
            {
                Data = _mapper.Map<IEnumerable<Transaction>>(transactions),
                Total = totalTransactions,
                Page = query.Page,
                PageSize = query.PageSize
            };

            return SuccessResp.Ok(result);
        }
    }
}
