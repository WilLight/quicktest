import { Switch, Route } from 'react-router-dom';
import { Header } from './components/Header';
import { Account } from './pages/Account';
import { Classroom } from './pages/Classroom';
import { Create } from './pages/Create';
import { Home } from './pages/Home';
import { Test } from './pages/Test';
import { Tests } from './pages/Tests';

function App() {
   return (
      <>
         <Header />
         <div className="container">
            <Switch>
               <Route path="/" component={Home} exact />
               <Route path="/tests" component={Tests} exact />
               <Route path="/account" component={Account} exact />
               <Route path="/classroom" component={Classroom} exact />
               <Route path="/create" component={Create} exact />
               <Route path="/t/:id" component={Test} exact />
            </Switch>
         </div>
      </>
   );
}

export default App;
