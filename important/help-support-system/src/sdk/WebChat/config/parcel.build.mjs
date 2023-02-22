import {Parcel} from '@parcel/core';
import rimraf from 'rimraf';
import commonConfig from './parcel.config.mjs';

const library = new Parcel({
  ...commonConfig,
  entries: 'src/lib.ts',
  targets: {
    library: {
      isLibrary: true,
      distDir: 'dist/lib',
      outputFormat: 'esmodule',
      engines: {
        node: '>=16'
      }
    }
  }
});

const bundler = new Parcel({
  ...commonConfig,
  entries: 'src/main.ts',
  targets: {
    main: {
      distDir: 'dist/main',
      engines: {
        browsers: ['last 1 Chrome version']
      }
    }
  }
});

async function run() {
  try {
    await rimraf('dist/');

    let {bundleGraph: libraryGraph, buildTime: libraryTime} = await library.run();
    let libraries = libraryGraph.getBundles();
    console.log(`✨ Built ${libraries.length} library in ${libraryTime}ms!`);
    let {bundleGraph, buildTime} = await bundler.run();
    let bundles = bundleGraph.getBundles();
    console.log(`✨ Built ${bundles.length} bundles in ${buildTime}ms!`);
    // console.log(bundles.filePath);
    // console.log(await outputFS.readFile(bundles.filePath, 'utf8'));
  } catch (err) {
    console.log(err.diagnostics);
  }
}

run();
