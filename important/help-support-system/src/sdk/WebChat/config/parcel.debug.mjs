import { Parcel } from '@parcel/core';
import fs from 'fs';
import commonConfig from './parcel.config.mjs';

const bundler = new Parcel({
  ...commonConfig,
  entries: 'src/main.ts',
  targets: {
    main: {
      distDir: 'dist/debug/',
      engines: {
        browsers: ['last 1 Chrome version']
      },
    }
  },
  serveOptions: {
    port: 3100
  },
  hmrOptions: {
    port: 3100
  }
});

const subscription = await bundler.watch((err, event) => {
  if (err) {
    // fatal error
    throw err;
  }

  if (event.type === 'buildSuccess') {
    const bundles = event.bundleGraph.getBundles();
    console.log(`✨ Built ${bundles.length} bundles in ${event.buildTime}ms!`);
    fs.writeFileSync('dist/index.html', fs.readFileSync('public/index.html'));
  } else if (event.type === 'buildFailure') {
    console.log(event.diagnostics);
  }
});

// some time later...
// await subscription.unsubscribe();