import * as alt from 'alt-server';

import { Doors } from '../shared/config.js';

alt.onClient('doorLock:server:updateState', (source, doorID, state) => {
    Doors[doorID].locked = state;
    alt.emitAllClients('doorLock:client:setState', doorID, state);
});