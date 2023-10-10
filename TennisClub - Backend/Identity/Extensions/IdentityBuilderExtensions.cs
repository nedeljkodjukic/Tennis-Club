﻿// Copyright (c) 2020 Allan Mobley. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Mobsites.Cosmos.Identity
{
    /// <summary>
    ///     Contains extension methods to <see cref="IdentityBuilder"/> for adding Cosmos stores.
    /// </summary>
    public static class IdentityBuilderExtensions
    {
        /// <summary>
        ///     Adds a Cosmos implementation of identity information stores.
        /// </summary>
        /// <typeparam name="TCosmosStorageProvider">The type representing a Cosmos storage provider.</typeparam>
        /// <param name="builder">The <see cref="IdentityBuilder"/> instance this method extends.</param>
        /// <returns>The <see cref="IdentityBuilder"/> instance this method extends.</returns>
        public static IdentityBuilder AddCosmosStores<TCosmosStorageProvider>(this IdentityBuilder builder)
            where TCosmosStorageProvider : IIdentityStorageProvider
            => builder.AddCosmosStores<TCosmosStorageProvider, IdentityUserClaim, IdentityUserRole, IdentityUserLogin, IdentityUserToken, IdentityRoleClaim>();

        /// <summary>
        ///     Adds a Cosmos implementation of identity information stores.
        /// </summary>
        /// <typeparam name="TCosmosStorageProvider">The type representing a Cosmos storage provider.</typeparam>
        /// <typeparam name="TUserClaim">The type representing a user claim.</typeparam>
        /// <typeparam name="TUserRole">The type representing a user role.</typeparam>
        /// <typeparam name="TUserLogin">The type representing a user external login.</typeparam>
        /// <typeparam name="TUserToken">The type representing a user token.</typeparam>
        /// <typeparam name="TRoleClaim">The type representing a role claim.</typeparam>
        /// <param name="builder">The <see cref="IdentityBuilder"/> instance this method extends.</param>
        /// <returns>The <see cref="IdentityBuilder"/> instance this method extends.</returns>
        public static IdentityBuilder AddCosmosStores<TCosmosStorageProvider,TUserClaim, TUserRole, TUserLogin, TUserToken, TRoleClaim>(this IdentityBuilder builder)
            where TCosmosStorageProvider : IIdentityStorageProvider
            where TUserClaim : IdentityUserClaim, new()
            where TUserRole : IdentityUserRole, new()
            where TUserLogin : IdentityUserLogin, new()
            where TUserToken : IdentityUserToken, new()
            where TRoleClaim : IdentityRoleClaim, new()
        {
            Type userType = builder.UserType;
            Type roleType = builder.RoleType;

            if (userType is null)
            {
                throw new InvalidOperationException($"Must provide an identity user of type {typeof(IdentityUser).FullName} or one that extends this type.");
            }
            else if (!typeof(IdentityUser).IsAssignableFrom(userType))
            {
                throw new InvalidOperationException($"{userType.Name} must extend {typeof(IdentityUser).FullName}.");
            }
            if (roleType is null)
            {
                roleType = typeof(IdentityRole);
            }
            else if (!typeof(IdentityRole).IsAssignableFrom(roleType))
            {
                throw new InvalidOperationException($"{roleType.Name} must extend {typeof(IdentityRole).FullName}.");
            }
            if (!typeof(IdentityUserClaim).IsAssignableFrom(typeof(TUserClaim)))
            {
                throw new InvalidOperationException($"{typeof(TUserClaim).Name} must extend {typeof(IdentityUserClaim).FullName}.");
            }
            if (!typeof(IdentityUserRole).IsAssignableFrom(typeof(TUserRole)))
            {
                throw new InvalidOperationException($"{typeof(TUserRole).Name} must extend {typeof(IdentityUserRole).FullName}.");
            }
            if (!typeof(IdentityUserLogin).IsAssignableFrom(typeof(TUserLogin)))
            {
                throw new InvalidOperationException($"{typeof(TUserLogin).Name} must extend {typeof(IdentityUserLogin).FullName}.");
            }
            if (!typeof(IdentityUserToken).IsAssignableFrom(typeof(TUserToken)))
            {
                throw new InvalidOperationException($"{typeof(TUserToken).Name} must extend {typeof(IdentityUserToken).FullName}.");
            }
            if (!typeof(IdentityRoleClaim).IsAssignableFrom(typeof(TRoleClaim)))
            {
                throw new InvalidOperationException($"{typeof(TRoleClaim).Name} must extend {typeof(IdentityRoleClaim).FullName}.");
            }

            Type userStoreType = typeof(UserStore<,,,,,,,>).MakeGenericType(
                typeof(TCosmosStorageProvider),
                userType,
                roleType, 
                typeof(TUserClaim), 
                typeof(TUserRole), 
                typeof(TUserLogin), 
                typeof(TUserToken), 
                typeof(TRoleClaim));

            Type roleStoreType = typeof(RoleStore<,,,>).MakeGenericType(
                typeof(TCosmosStorageProvider),
                roleType,
                typeof(TUserRole),
                typeof(TRoleClaim));

            builder.Services.TryAddScoped(typeof(IUserStore<>).MakeGenericType(userType), userStoreType);
            builder.Services.TryAddScoped(typeof(IRoleStore<>).MakeGenericType(roleType), roleStoreType);

            return builder;
        }
    }
}
