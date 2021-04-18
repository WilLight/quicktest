import { Switch, Route } from 'react-router-dom';
import { Header } from './components/Header';
import Modals from './modules/Modals';

function App() {
   return (
      <>
         <Header />
         <div className="container">
            <Switch>
               <Route path="/modules" component={Modals} exact />
            </Switch>
         </div>
      </>
   );
}

export default App;
