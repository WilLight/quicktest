import { Switch, Route } from 'react-router-dom';
import { Header } from './components/Header';
import { Account } from './pages/Account';
import { Classroom } from './pages/Classroom';
import {Modules} from './pages/Modules';

function App() {
   return (
      <>
         <Header />
         <div className="container">
            <Switch>
               <Route path="/modules" component={Modules} exact />
               <Route path="/account" component={Account} exact />
               <Route path="/classroom" component={Classroom} exact />
            </Switch>
         </div>
      </>
   );
}

export default App;
