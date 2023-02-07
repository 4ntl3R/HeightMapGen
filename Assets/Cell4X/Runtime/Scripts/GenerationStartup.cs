using System;
using Cell4X.Runtime.Scripts.Controllers;
using Cell4X.Runtime.Scripts.Models;
using Cell4X.Runtime.Scripts.Views.Interfaces;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Cell4X.Runtime.Scripts
{
    public class GenerationStartup : IStartable, IDisposable
    {
        private readonly IGridView _gridView;
        private readonly DiamondSquarePlatesModel _generationModel;
        private readonly InputFieldController _inputController;
        private readonly LifetimeScope _scope;

        public GenerationStartup(IGridView view, DiamondSquarePlatesModel generationModel, InputFieldController input, 
            LifetimeScope scope)
        {
            _gridView = view;
            _generationModel = generationModel;
            _inputController = input;
            _scope = scope;
        }
        
        public void Start()
        {
            _scope.Container.Resolve<DiamondSquarePlatesModel>();
            _scope.Container.Resolve<IGridView>();
            _scope.Container.Resolve<InputFieldController>();
            _generationModel.HeightsGenerated += _gridView.DrawGrid;
            _inputController.GenerationButtonPressed += _generationModel.Generate;
            LoadAllInputParameters();
        }

        public void Dispose()
        {
            _generationModel.HeightsGenerated -= _gridView.DrawGrid;
            _inputController.GenerationButtonPressed -= _generationModel.Generate;
        }

        private void LoadAllInputParameters()
        {
            _inputController.OnInputFieldUpdate("-1");
            _inputController.OnInputFieldUpdate("1");
            _inputController.OnInputFieldUpdate("2");
            _inputController.OnInputFieldUpdate("3");
            _inputController.OnInputFieldUpdate("4");
            _inputController.OnInputFieldUpdate("5");
            _inputController.OnInputFieldUpdate("6");
            _inputController.OnInputFieldUpdate("7");
            _inputController.OnInputFieldUpdate("8");
        }
        
    }
}
