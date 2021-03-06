﻿using pdfforge.PDFCreator.Conversion.Jobs;
using pdfforge.PDFCreator.Conversion.Settings;
using pdfforge.PDFCreator.UI.Presentation.Helper.Translation;
using pdfforge.PDFCreator.UI.Presentation.ViewModelBases;
using System;
using System.ComponentModel;
using Translatable;

namespace pdfforge.PDFCreator.UI.Presentation.UserControls.Profiles
{
    public class ProfileUserControlViewModel<TTranslation> : TranslatableViewModelBase<TTranslation> where TTranslation : ITranslatable, new()
    {
        private readonly ISelectedProfileProvider _selectedProfileProvider;
        private readonly IDispatcher _dispatcher;
        public ConversionProfile CurrentProfile => _selectedProfileProvider.SelectedProfile;

        public event EventHandler CurrentProfileChanged;

        public ProfileUserControlViewModel(ITranslationUpdater translationUpdater, ISelectedProfileProvider selectedProfile, IDispatcher dispatcher) : base(translationUpdater)
        {
            _selectedProfileProvider = selectedProfile;
            _dispatcher = dispatcher;
            selectedProfile.SelectedProfileChanged += OnCurrentProfileChanged;
            selectedProfile.SettingsChanged += OnCurrentSettingsChanged;
        }

        private void OnCurrentSettingsChanged(object sender, EventArgs e)
        {
            OnCurrentProfileChanged(this, null);
        }

        protected virtual void OnCurrentProfileChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            _dispatcher?.BeginInvoke(() =>
            {
                CurrentProfileChanged?.Invoke(this, EventArgs.Empty);
                RaisePropertyChanged(nameof(CurrentProfile));
            });
        }
    }
}
