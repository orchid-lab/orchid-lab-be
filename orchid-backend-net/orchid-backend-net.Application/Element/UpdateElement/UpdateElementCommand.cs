using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Element.UpdateElement
{
    public class UpdateElementCommand : IRequest<string>, ICommand
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public UpdateElementCommand(string id, string name, string description)
        {
            ID = id;
            Name = name;
            Description = description;
        }
        public UpdateElementCommand() { }
    }

    public class UpdateElementCommandHandler : IRequestHandler<UpdateElementCommand, string>
    {
        private readonly IElementRepositoty _elementRepositoty;
        public UpdateElementCommandHandler(IElementRepositoty elementRepositoty)
        {
            _elementRepositoty = elementRepositoty;
        }

        public async Task<string> Handle(UpdateElementCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var element = await this._elementRepositoty.FindAsync(x => x.ID.Equals(request.ID) && x.Status == true, cancellationToken);
                element.Description = request.Description;
                element.Name = request.Name;
                this._elementRepositoty.Update(element);
                return await this._elementRepositoty.UnitOfWork.SaveChangesAsync(cancellationToken) > 0 ? $"Updated element name {request.Name}" : "Failed update element";
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
