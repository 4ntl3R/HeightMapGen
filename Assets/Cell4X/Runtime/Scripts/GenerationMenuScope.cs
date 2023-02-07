using System;
using Cell4X.Runtime.Scripts.Controllers;
using Cell4X.Runtime.Scripts.Data;
using Cell4X.Runtime.Scripts.Factories;
using Cell4X.Runtime.Scripts.Factories.Interfaces;
using Cell4X.Runtime.Scripts.Models;
using Cell4X.Runtime.Scripts.Views;
using Cell4X.Runtime.Scripts.Views.Interfaces;
using UnityEngine;
using UnityEngine.Tilemaps;
using VContainer;
using VContainer.Unity;

namespace Cell4X.Runtime.Scripts
{
    public class GenerationMenuScope : LifetimeScope
    {
        [SerializeField] 
        private InputFieldController inputController;
        
        [SerializeField] 
        private Tilemap tilemap;

        [SerializeField] 
        private Tile baseTile;

        [SerializeField] 
        private HeightColorsData colorsData;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<TectonicHeightGenerator>(Lifetime.Singleton);
            builder.Register<DiamondSquareDownscaleFactory>(Lifetime.Singleton);
            builder.Register<SimpleTectonicPlatesFactory>(Lifetime.Singleton).As<ITectonicPlatesFactory>();
            builder.Register<DiamondSquareHeightGenerator>(Lifetime.Singleton).As<ILandscapeHeightsFactory>();
            builder.Register<SimpleLayerMerger>(Lifetime.Singleton).As<IHeightsLayersMergeFactory>();
            
            builder.Register<DiamondSquarePlatesModel>(Lifetime.Singleton);

            builder.RegisterComponent(inputController);
            builder.RegisterComponent(tilemap);

            builder.RegisterInstance(baseTile);
            builder.RegisterInstance(colorsData);

            builder.Register<GridTilemapView>(Lifetime.Singleton).As<IGridView>();

            builder.RegisterEntryPoint<GenerationStartup>().As<GenerationStartup, IDisposable>();
        }
    }
}
