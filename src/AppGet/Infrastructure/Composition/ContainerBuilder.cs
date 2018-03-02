﻿using AppGet.Commands;
using AppGet.Commands.CreateManifest;
using AppGet.Commands.Install;
using AppGet.Commands.List;
using AppGet.Commands.Search;
using AppGet.Commands.Uninstall;
using AppGet.Commands.ViewManifest;
using AppGet.Commands.WindowsInstallerSearch;
using AppGet.CreatePackage.Installer;
using AppGet.CreatePackage.Installer.Prompts;
using AppGet.CreatePackage.Root;
using AppGet.CreatePackage.Root.Extractors;
using AppGet.CreatePackage.Root.Prompts;
using AppGet.Crypto.Hash;
using AppGet.Crypto.Hash.Algorithms;
using AppGet.FileTransfer;
using AppGet.FileTransfer.Protocols;
using AppGet.Installers;
using AppGet.Installers.Custom;
using AppGet.Installers.Inno;
using AppGet.Installers.InstallBuilder;
using AppGet.Installers.InstallShield;
using AppGet.Installers.Msi;
using AppGet.Installers.Nsis;
using AppGet.Installers.Squirrel;
using AppGet.Installers.Zip;
using AppGet.PackageRepository;
using NLog;

namespace AppGet.Infrastructure.Composition
{
    public static class ContainerBuilder
    {
        public static TinyIoCContainer Build()
        {
            var container = new TinyIoCContainer();

            var logger = LogManager.GetLogger("appget");

            container.AutoRegister(new[] { typeof(ContainerBuilder).Assembly });
            container.Register(logger);

            RegisterLists(container);

            return container;
        }

        private static void RegisterLists(TinyIoCContainer container)
        {
            container.RegisterMultiple<ICommandHandler>(new[]
            {
                typeof(ViewManifestCommandHandler),
                typeof(SearchCommandHandler),
                typeof(ListCommandHandler),
                typeof(InstallCommandHandler),
                typeof(WindowsInstallerSearchCommandHandler),
                typeof(UninstallCommandHandler),
                typeof(CreateManifestCommandHandler)
            });

            container.RegisterMultiple<IInstallerWhisperer>(new[]
            {
                typeof(CustomWhisperer),
                typeof(InnoWhisperer),
                typeof(InstallBuilderWhisperer),
                typeof(InstallShieldWhisperer),
                typeof(MsiWhisperer),
                typeof(NsisWhisperer),
                typeof(SquirrelWhisperer),
                typeof(ZipWhisperer),
            });

            container.RegisterMultiple<ICheckSum>(new[]
            {
                typeof(Sha256Hash),
                typeof(Sha1Hash),
                typeof(Md5Hash),
            });

            container.RegisterMultiple<IExtractToManifestRoot>(new[]
            {
                typeof(FileVersionInfoExtractor),
                typeof(GithubExtractor),
                typeof(InstallMethodExtractor),
                typeof(NameExtractor),
                typeof(PackageIdExtractor),
                typeof(SourceforgeExtractor),
                typeof(SquirrelExtractor),
                typeof(AppGet.CreatePackage.Root.Extractors.UrlExtractor),
            });

            container.RegisterMultiple<IManifestPrompt>(new[]
            {
                typeof(ProductNamePrompt),
                typeof(PackageIdPrompt),
                typeof(VersionPrompt),
                typeof(HomePagePrompt),
                typeof(LicensePrompt),
                typeof(InstallMethodPrompt),
                typeof(VersionTagPrompt),
            });



            container.RegisterMultiple<IExtractToInstaller>(new[]
            {
                typeof(AppGet.CreatePackage.Installer.Extractors.UrlExtractor)
            });

            container.RegisterMultiple<IInstallerPrompt>(new[]
            {
                typeof(ArchitecturePrompt),
                typeof(MinWindowsVersionPrompt)
            });

            container.RegisterMultiple<IDetectInstallMethod>(new[]
            {
                typeof(MsiDetector),
                typeof(SquirrelDetector),
                typeof(NsisDetector),
                typeof(InnoDetector),
                typeof(InstallBuilderDetector)
            });


            container.RegisterMultiple<IFileTransferClient>(new[]
            {
                typeof(HttpFileTransferClient),
                typeof(WindowsPathFileTransferClient)
            });


            container.RegisterMultiple<IPackageRepository>(new[]
            {
                typeof(LocalPackageRepository),
                typeof(OfficialPackageRepository)
            });

            container.Register<IPackageRepository, AggregateRepository>();
        }

    }
}