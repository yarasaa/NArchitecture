using Application.Features.Models.Models;
using Application.Services.Repositories;
using AutoMapper;
using Core.Application.Requests;
using Core.Persistence.Paging;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Models.Queries.GetListModel
{
    public class GetListModelQuery:IRequest<ModelListModel>
    {
        public PageRequest PageRequest { get; set; }

        public class GetListModelQueryHandler : IRequestHandler<GetListModelQuery, ModelListModel>
        {
            private readonly IMapper _mappper;
            private readonly IModelRepository _modelRepository;

            public GetListModelQueryHandler(IMapper mappper, IModelRepository modelRepository)
            {
                _mappper = mappper;
                _modelRepository = modelRepository;
            }

            public async Task<ModelListModel> Handle(GetListModelQuery request, CancellationToken cancellationToken)
            {
               IPaginate<Model> models=await  _modelRepository.GetListAsync(include:
                                               m => m.Include(c=>c.Brand),
                                               index:request.PageRequest.Page,
                                               size:request.PageRequest.PageSize
                                               );

                ModelListModel mappedModels = _mappper.Map<ModelListModel>(models);
                return mappedModels;
            }
        }
    }
}
