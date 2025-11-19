using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Domain.Entities;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Images.GetInfor
{
    public class GetImagesInforQuery(string id) : IRequest<ImagesDTO>, IQuery
    {
        public string ID { get; set; } = id;
    }

    internal class GetImgesInforQueryHandler(IImageRepository imageRepository) : IRequestHandler<GetImagesInforQuery, ImagesDTO>
    {
        public async Task<ImagesDTO> Handle(GetImagesInforQuery request, CancellationToken cancellationToken)
        {
            IQueryable<Imgs> queryOptions(IQueryable<Imgs> query)
            {
                query = query.Where(x => x.ID == request.ID && x.Status == true);
                return query;
            }
            var image = await imageRepository.FindProjectToAsync<ImagesDTO>(
                queryOptions,
                cancellationToken: cancellationToken);
            return image;
        }
    }
}
