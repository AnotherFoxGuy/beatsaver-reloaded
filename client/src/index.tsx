import React from 'react'
import { render } from 'react-dom'
import { Provider } from 'react-redux'

import { App } from './ts/App'
import { history, store } from './ts/store'

import '@lolpants/bulma/css/bulma.css'
import { ConnectedRouter } from 'connected-react-router'

render(
  <Provider store={store}>
    <ConnectedRouter history={history}>
      <App />
    </ConnectedRouter>
  </Provider>,
  document.getElementById('root')
)
