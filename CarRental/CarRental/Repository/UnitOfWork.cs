using CarRental.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarRental.Repository
{
    public class UnitOfWork : IDisposable
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private GenericRepository<Car> carRepository;
        private GenericRepository<CarBody> carBodyRepository;
        private GenericRepository<Transmission> transmissionRepository;
        private GenericRepository<CarClass> carClassRepository;
        private GenericRepository<ReservForm> formRepository;
        private GenericRepository<Place> placeRepository;

        public GenericRepository<Car> CarRepository
        {
            get
            {
                if(this.carRepository == null)
                {
                    this.carRepository = new GenericRepository<Car>(db);
                }
                return carRepository;
            }
        }

        public GenericRepository<CarBody> CarBodyRepository
        {
            get
            {
                if (this.carBodyRepository == null)
                {
                    this.carBodyRepository = new GenericRepository<CarBody>(db);
                }
                return carBodyRepository;
            }
        }

        public GenericRepository<CarClass> CarClassRepository
        {
            get
            {
                if (this.carClassRepository == null)
                {
                    this.carClassRepository = new GenericRepository<CarClass>(db);
                }
                return carClassRepository;
            }
        }

        public GenericRepository<Transmission> TransmissionRepository
        {
            get
            {
                if (this.transmissionRepository == null)
                {
                    this.transmissionRepository = new GenericRepository<Transmission>(db);
                }
                return transmissionRepository;
            }
        }

        public GenericRepository<ReservForm> ReservFormRepository
        {
            get
            {
                if (this.formRepository == null)
                {
                    this.formRepository = new GenericRepository<ReservForm>(db);
                }
                return formRepository;
            }
        }

        public GenericRepository<Place> PlaceRepository
        {
            get
            {
                if (this.placeRepository == null)
                {
                    this.placeRepository = new GenericRepository<Place>(db);
                }
                return placeRepository;
            }
        }

        public void Save()
        {
            db.SaveChanges();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~UnitOfWork() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}