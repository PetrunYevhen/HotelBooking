// using Autofac;
// using BookingManagement.Application.Gateway;
// using HotelBooking.API.Composition.Room;
//
// namespace HotelBooking.API.Composition;
//
// public class CompositionModule : Module
// {
//
//     protected override void Load(ContainerBuilder builder)
//     {
//         var assembly = typeof(CompositionModule).Assembly;
//         
//         builder.RegisterType<RoomGateway>()
//             .As<IRoomGateway>()
//             .InstancePerLifetimeScope();
//     }
//
// }