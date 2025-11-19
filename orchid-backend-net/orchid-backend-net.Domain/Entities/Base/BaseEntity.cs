using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace orchid_backend_net.Domain.Entities.Base
{
    public abstract class BaseEntity<T> : IDisposable
    {
        [Key]
        public abstract T ID { get; set; }

        [NotMapped]
        private bool IsDisposed { get; set; }

        public void Dispose()
        {
            Dispose(isDisposing: true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool isDisposing)
        {
            if (!IsDisposed)
            {
                if (isDisposing)
                {
                    DisposeUnmanagedResources();
                }

                IsDisposed = true;
            }
        }

        protected virtual void DisposeUnmanagedResources()
        {
        }

        ~BaseEntity()
        {
            Dispose(isDisposing: false);
        }
    }
}
