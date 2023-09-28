import { ConfigurationLoader } from '../../operation';
import { catchError, of } from 'rxjs';

export function AppInitializer(configurationLoader: ConfigurationLoader) {
  return () => configurationLoader.waitForConfigurationToLoad()
    .pipe(
      // catch error to start app on success or failure
      catchError(() => of())
    );
}
